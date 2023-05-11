using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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
            crumbs.Add(new Breadcrumb { text = "Forum", url = "/forum" });
            this.activityService = activityService;
            this.userService = userService;
            this.forumService = forumService;
        }
        [Authorize(Policy = Claims.Forum.ReadPost)]
        public IActionResult index()
        {
            ViewBag.crumbs = crumbs;
            List<string> courses = forumService.getCategories(userService.getCurrentUser().Result);
            return View("mainPage", courses);
        }

        public IActionResult category([FromQuery] int category, [FromQuery] int page = 1)
        {
            Thesis.Models.Course course = courseService.getCourseById(category);
            if (course == null)
            {
                return LocalRedirect("/Home/showError");
            }
            crumbs.Add(new Breadcrumb { text = course.name, current = true });
            ViewBag.crumbs = crumbs;
            List<Post> posts = forumService.getPostsForCategory(page, PAGE_SIZE, category);
            ApplicationUser currentUser = userService.getCurrentUser().Result;
            ForumUser user = new ForumUser
            {
                userId = currentUser.Id,
                canEdit = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.Forum.EditYour),
                canEditAny = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.Forum.EditAny),
                canDelete = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.Forum.Delete)
            };
            return View("categoryPage.cshtml", (posts, user));
        }

        [Authorize(Policy = Claims.Forum.WritePost)]
        public IActionResult add([FromQuery] int activityId = 0)
        {
            if (activityId != 0)
            {
                Activity activity = activityService.GetActivityByIdWithParent(activityId);
                if (!checkAllowedByActivity(activityId))
                {
                    return LocalRedirect("/Home/showError");
                }
                crumbs.Add(new Breadcrumb { text = activity.course.name, url = "/forum/forum/category?category=" + activity.courseId });
                crumbs.Add(new Breadcrumb { text = "Add post", current = true });
                ViewBag.crumbs = crumbs;
            }
            return View("writePost", activityId);
        }

        [Authorize(Policy = Claims.Forum.WritePost)]
        [HttpPost]
        public OperationResult save([FromBody] Post post)
        {
            if (post.activityId != 0)
            {
                if (post.activityId != null && !checkAllowedByActivity((int)post.activityId))
                {
                    return new OperationResult { success = false, text = "Activity couldn't be found or you don't have access to it" };
                }
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

        [Authorize(Policy = Claims.Forum.EditYour)]
        [HttpPost]
        public OperationResult update([FromQuery] int /*post*/id, [FromBody] string content)
        {
            Post post = forumService.getPostWithComments(id);
            if (post == null)
            {
                return new OperationResult { success = false, text = "Post couldn't be found or you don't have access to it" };
            }
            if (post.authorId == userService.getCurrentUser().Result.Id)
            {
                return new OperationResult { success = false, text = "Activity related to post couldn't be found or you don't have access to it" };
            }
            post.edited = true;
            post.editDate = DateTime.Now;
            post.editor = userService.getCurrentUser().Result;
            post.content = content;
            string result = forumService.update(post);
            if (result == "")
            {
                return new OperationResult { success = true, text = "Post updated successfully", data = new List<string> { "/forum/forum/post?id=" + post.id } };
            }
            return new OperationResult { success = false, text = result };
        }

        [Authorize(Policy = Claims.Forum.EditAny)]
        [HttpPost]
        public OperationResult edit([FromQuery] int /*post*/id, [FromBody] string content)
        {
            Post post = forumService.getPostWithComments(id);
            if (post == null)
            {
                return new OperationResult { success = false, text = "Post couldn't be found or you don't have access to it" };
            }
            post.edited = true;
            post.editDate = DateTime.Now;
            post.editor = userService.getCurrentUser().Result;
            post.content = content;
            string result = forumService.update(post);
            if (result == "")
            {
                return new OperationResult { success = true, text = "Post updated successfully", data = new List<string> { "/forum/forum/post?id=" + post.id } };
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
            if (post.activityId != null && checkAllowedByActivity((int)post.activityId))
            {
                return LocalRedirect("/Home/showError");
            }
            crumbs.Add(new Breadcrumb { text = post.activity.course.name, url = "/forum/forum/category?category=" + post.activity.courseId });
            crumbs.Add(new Breadcrumb { text = post.title, current = true });
            ViewBag.crumbs = crumbs;
            return View("post", post);
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
            if (post.activityId != null && checkAllowedByActivity((int)post.activityId))
            {
                return new OperationResult { success = false, text = "You don't have access to this post" };
            }
            PostComment comment = new PostComment { author = userService.getCurrentUser().Result, post = post, postId = id, edited = false, date = DateTime.Now, editDate = DateTime.Now, content = content };
            string result = forumService.saveComment(comment);
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
            comment.editor = userService.getCurrentUser().Result;
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
            comment.editor = userService.getCurrentUser().Result;
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
        public OperationResult deletePost(int id)
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
        public OperationResult deleteComment(int id)
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
            ApplicationUser user = userService.getCurrentUser().Result;
            Activity activity = activityService.GetActivityByIdWithParent(id);
            if (activity == null)
            {
                return false;
            }
            return courseService.checkIfExists(activity.courseId) || courseService.checkCourseAccess(user, id);
        }
    }
}
