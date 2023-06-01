using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Policy;
using Thesis.Areas.Forum.Models;
using Thesis.Areas.Identity.Constants;
using Thesis.Migrations;
using Thesis.Models;
using Thesis.Services;
using Activity = Thesis.Models.Activity;

namespace Thesis.Areas.Forum.Controllers
{
    [Area("Forum")]
    public class ForumController : Controller
    {
        private readonly int PAGE_SIZE = 10;
        private readonly CourseService courseService;
        private readonly ActivityService activityService;
        private readonly UserService userService;
        private readonly ForumService forumService;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public ForumController(CourseService courseService, ActivityService activityService, UserService userService, ForumService forumService)
        {
            this.courseService = courseService;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
            crumbs.Add(new Breadcrumb { text = "Forum", url = "/forum/forum" });
            this.activityService = activityService;
            this.userService = userService;
            this.forumService = forumService;
        }
        [Authorize(Policy = Claims.Forum.ReadPost)]
        public IActionResult index()
        {
            ViewBag.crumbs = crumbs;
            List<CategoryModel> courses = forumService.getCategories();
            return View("mainPage", courses);
        }

        public IActionResult category([FromQuery] int /*category*/id, [FromQuery] int page = 1)
        {
            Thesis.Models.Course course = courseService.getCourseWithDefault(id);
            if (course == null)
            {
                return LocalRedirect("/Home/showError");
            }
            crumbs.Add(new Breadcrumb { text = course.name, current = true });
            ViewBag.crumbs = crumbs;
            List<Post> posts = forumService.getPostsForCategory(page, PAGE_SIZE, id);
            int pageCount = forumService.getPageCount(PAGE_SIZE, id);
            ForumUser user = createForumUser();
            return View("categoryPage", (posts, user, id, new Pagination(page, pageCount, "/forum/forum/category?id=" + id + "&page={0}")));
        }

        [Authorize(Policy = Claims.Forum.WritePost)]
        public IActionResult add([FromQuery] int courseId = -1, [FromQuery] int activityId = -1)
        {
            Thesis.Models.Course course = new Thesis.Models.Course();
            if (courseId != -1)
            {
                if (!checkAllowedByCourse(courseId))
                {
                    return LocalRedirect("/Home/showError");
                }
                course = courseService.getCourseById(courseId);
                activityId = 0;
            }
            if (activityId != -1)
            {
                if (!checkAllowedByActivity(activityId))
                {
                    return LocalRedirect("/Home/showError");
                } 
                if (activityId != 0)
                {
                    Post post = forumService.getPostByActivity(activityId);
                    if (post != null)
                    {
                        return LocalRedirect("/Forum/Forum/post?id=" + post.id);
                    }
                    course = activityService.GetActivityByIdWithParent(activityId).course;
                    courseId = course.id;
                }
            }
            crumbs.Add(new Breadcrumb { text = course.name, url = "/forum/forum/category?id=" + courseId });
            crumbs.Add(new Breadcrumb { text = "Add post", current = true });
            ViewBag.crumbs = crumbs;
            return View("writePost", (courseId, activityId));
        }

        [Authorize(Policy = Claims.Forum.WritePost)]
        [HttpPost]
        public OperationResult save([FromBody] Post post)
        {
            if (post.activityId != -1)
            {
                if (!checkAllowedByActivity((int)post.activityId))
                {
                    return new OperationResult { success = false, text = "Activity couldn't be found or you don't have access to it" };
                }

                if (forumService.getPostByActivity((int)post.activityId) != null && post.activityId > 0)
                {
                    return new OperationResult { success = false, text = "There already exists a post about this activity. Try visiting it." };
                }
            }
            if (post.courseId != -1 && !checkAllowedByCourse((int)post.courseId))
            {
                return new OperationResult { success = false, text = "Course couldn't be found or you don't have access to it" };
            }
            post.author = userService.getCurrentUser().Result;
            post.edited = false;
            post.date = DateTime.Now;
            post.editDate = DateTime.Now;
            string result = forumService.save(post);
            if (result == "")
            {
                return new OperationResult { success = true, text = "Post added successfully", data = new List<string> { "/forum/forum/post?id=" + post.id } };
            }
            return new OperationResult { success = false, text = result };
        }

        [Authorize]
        public IActionResult edit([FromQuery] int /*post*/id)
        {
            Post editedPost = forumService.getPostWithComments(id);
            if (editedPost == null)
            {
                return LocalRedirect("/Home/showError");
            }
            if (editedPost.activityId != 0 && !checkAllowedByActivity((int)editedPost.activityId))
            {
                return LocalRedirect("/Home/showError");
            }
            if (editedPost.courseId != 0 && !checkAllowedByCourse((int)editedPost.courseId))
            {
                return LocalRedirect("/Home/showError");
            }
            crumbs.Add(new Breadcrumb { text = editedPost.course.name, url = "/forum/forum/category?id=" + editedPost.courseId });
            crumbs.Add(new Breadcrumb { text = "Add post", current = true });
            ViewBag.crumbs = crumbs;
            ForumUser user = createForumUser();
            bool allowedToEdit = user.canEditAny || (user.canEdit && editedPost.authorId == user.userId);
            if (!allowedToEdit)
            {
                return LocalRedirect("/Home/showError");
            }
            string url = "/Forum/Forum/submitEdit";
            if (user.canEdit && editedPost.authorId == user.userId)
            {
                url = "/Forum/Forum/submitUpdate";
            }
            return View("edit", (editedPost, user, url));
        }

        [Authorize(Policy = Claims.Forum.EditYour)]
        [HttpPost]
        public OperationResult submitUpdate([FromBody] Post post)
        {
            Post original = forumService.getPostWithComments(post.id);
            if (original == null)
            {
                return new OperationResult { success = false, text = "Post couldn't be found or you don't have access to it" };
            }
            if (original.authorId != userService.getCurrentUser().Result.Id)
            {
                return new OperationResult { success = false, text = "Activity related to post couldn't be found or you don't have access to it" };
            }
            original.edited = true;
            original.editDate = DateTime.Now;
            original.editorId = userService.getCurrentUser().Result.Id;
            original.content = post.content;
            original.title = post.title;
            string result = forumService.update(original);
            if (result == "")
            {
                return new OperationResult { success = true, text = "Post updated successfully", data = new List<string> { "/forum/forum/post?id=" + original.id } };
            }
            return new OperationResult { success = false, text = result };
        }

        [Authorize(Policy = Claims.Forum.EditAny)]
        [HttpPost]
        public OperationResult submitEdit([FromBody] Post post)
        {
            Post original = forumService.getPostWithComments(post.id);
            if (original == null)
            {
                return new OperationResult { success = false, text = "Post couldn't be found or you don't have access to it" };
            }
            original.edited = true;
            original.editDate = DateTime.Now;
            original.editorId = userService.getCurrentUser().Result.Id;
            original.content = post.content;
            original.title = post.title;
            string result = forumService.update(original);
            if (result == "")
            {
                return new OperationResult { success = true, text = "Post updated successfully", data = new List<string> { "/forum/forum/post?id=" + original.id } };
            }
            return new OperationResult { success = false, text = result };
        }

        [Authorize(Policy = Claims.Forum.ReadPost)]
        public IActionResult post([FromQuery] int id)
        {
            Post post = forumService.getPostWithComments(id);
            if (post == null)
            {
                return LocalRedirect("/Home/showError");
            }
            if (post.activityId != 0 && !checkAllowedByActivity((int)post.activityId))
            {
                return LocalRedirect("/Home/showError");
            }
            if (post.courseId != 0 && !checkAllowedByCourse((int)post.courseId))
            {
                return LocalRedirect("/Home/showError");
            }
            crumbs.Add(new Breadcrumb { text = post.course.name, url = "/forum/forum/category?id=" + post.courseId });
            crumbs.Add(new Breadcrumb { text = post.title, current = true });
            ViewBag.crumbs = crumbs;
            return View("post", (post, createForumUser()));
        }

        [Authorize(Policy = Claims.Forum.CommentPost)]
        [HttpPost]
        public OperationResult saveComment([FromQuery] int /*post*/id, [FromBody] string content)
        {
            Post post = forumService.getPostWithComments(id);
            if (post == null)
            {
                return new OperationResult { success = false, text = "Post not found" };
            }
            if (post.activityId != 0 && !checkAllowedByActivity((int)post.activityId))
            {
                return new OperationResult { success = false, text = "You don't have access to this post" };
            }
            if (post.courseId != 0 && !checkAllowedByCourse((int)post.courseId))
            {
                return new OperationResult { success = false, text = "You don't have access to this post" };
            }
            post.editDate = DateTime.Now;
            PostComment comment = new PostComment { author = userService.getCurrentUser().Result, post = post, postId = id, edited = false, date = DateTime.Now, editDate = DateTime.Now, content = content };
            string result = forumService.saveComment(comment);
            if (result == "")
            {
                result = forumService.update(post);
            }
            if (result == "")
            {
                return new OperationResult
                {
                    success = true,
                    text = "Comment added successfully",
                    data = new List<string> { string.Format("/forum/forum/post?id={0}#comment{1}", post.id, comment.id) }
                };
            }
            return new OperationResult
            {
                success = false,
                text = result
            };
        }

        [Authorize(Policy = Claims.Forum.EditYour)]
        [HttpPost]
        public OperationResult updateComment([FromQuery] int /*comment*/id, [FromBody] string content)
        {
            PostComment comment = forumService.GetComment(id);
            if (comment == null)
            {
                return new OperationResult { success = false, text = "Comment not found" };
            }
            if (comment.authorId != userService.getCurrentUser().Result.Id)
            {
                return new OperationResult { success = false, text = "You can't edit this comment" };
            }
            comment.editDate = DateTime.Now;
            comment.edited = true;
            comment.editorId = userService.getCurrentUser().Result.Id;
            comment.content = content;
            string result = forumService.updateComment(comment);
            if (result == "")
            {
                return new OperationResult
                {
                    success = true,
                    text = "Comment updated successfully",
                    data = new List<string> { string.Format("/forum/forum/post?id={0}#comment{1}", comment.post.id, comment.id) }
                };
            }
            return new OperationResult
            {
                success = false,
                text = result
            };
        }

        [Authorize(Policy = Claims.Forum.EditAny)]
        [HttpPost]
        public OperationResult editComment([FromQuery] int /*comment*/id, [FromBody] string content)
        {
            PostComment comment = forumService.GetComment(id);
            if (comment == null)
            {
                return new OperationResult { success = false, text = "Comment not found" };
            }
            comment.editDate = DateTime.Now;
            comment.edited = true;
            comment.editorId = userService.getCurrentUser().Result.Id;
            comment.content = content;
            string result = forumService.updateComment(comment);
            if (result == "")
            {
                return new OperationResult
                {
                    success = true,
                    text = "Comment updated successfully",
                    data = new List<string> { string.Format("/forum/forum/post?id={0}#comment{1}", comment.post.id, comment.id) }
                };
            }
            return new OperationResult
            {
                success = false,
                text = result
            };
        }


        [Authorize(Policy = Claims.Forum.Delete)]
        [HttpPost]
        public OperationResult deletePost([FromBody] int id)
        {
            Post post = forumService.getPostWithComments(id);
            if (post == null)
            {
                return new OperationResult { success = false, text = "Post couldn't be found" };
            }
            forumService.deletePost(post);
            return new OperationResult { success = true, text = "Post deleted successfully" };
        }

        [Authorize(Policy = Claims.Forum.Delete)]
        [HttpPost]
        public OperationResult deleteComment([FromBody] int id)
        {
            PostComment comment = forumService.GetComment(id);
            if (comment == null)
            {
                return new OperationResult { success = false, text = "Comment couldn't be found" };
            }
            forumService.deleteComment(comment);
            return new OperationResult { success = true, text = "Comment deleted successfully" };
        }

        private bool checkAllowedByActivity(int id)
        {
            if (id < 1)
            {
                return true;
            }
            ApplicationUser user = userService.getCurrentUser().Result;
            Activity activity = activityService.GetActivityByIdWithParent(id);
            if (activity == null)
            {
                return false;
            }
            return courseService.checkIfExists(activity.courseId) && courseService.checkCourseAccess(user, activity.courseId);
        }
        private bool checkAllowedByCourse(int id)
        {
            if (id < 1)
            {
                return true;
            }
            ApplicationUser user = userService.getCurrentUser().Result;
            Thesis.Models.Course course = courseService.getCourseById(id);
            if (course == null)
            {
                return false;
            }
            return courseService.checkIfExists(id) && courseService.checkCourseAccess(user, id);
        }


        private ForumUser createForumUser()
        {
            ApplicationUser currentUser = userService.getCurrentUser().Result;
            return new ForumUser()
            {
                userId = currentUser.Id,
                canAdd = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.Forum.WritePost),
                canEdit = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.Forum.EditYour),
                canEditAny = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.Forum.EditAny),
                canDelete = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.Forum.Delete),
                canComment = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.Forum.CommentPost)
            };
        }
    }
}
