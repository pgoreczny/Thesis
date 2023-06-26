using Microsoft.EntityFrameworkCore;
using Thesis.Areas.Forum.Models;
using Thesis.database;
using Thesis.Models;

namespace Thesis.Services
{
    public class ForumService
    {
        private readonly CoursesDBContext context;
        private readonly UserService userService;
        public ForumService(CoursesDBContext context, UserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public List<Post> getPostsForUser(int page, int pageSize)
        {
            int offset = Math.Max(page - 1, 0) * pageSize;
            List<int> userCourses = userService.getCurrentUser().Result.CourseApplicationUsers
                .Select(cau => cau.CourseId)
                .Distinct()
                .ToList();
            List<int> userActivities = context.activities
                .Where(activity => userCourses.Contains(activity.courseId))
                .Select(activity => activity.id)
                .ToList();
            userActivities.Add(0);
            return context.posts
                .Where(post => post.activityId == null || userActivities.Contains((int)post.activityId))
                .Include(post => post.author)
                .Include(post => post.comments)
                .OrderByDescending(post => post.editDate)
                .Skip(offset)
                .Take(pageSize)
                .ToList();
        }

        public List<Post> getPostsForCategory(int page, int pageSize, int category)
        {
            int offset = Math.Max(page - 1, 0) * pageSize;
            return context.posts
                .Where(post => post.courseId == category)
                .Include(post => post.author)
                .Include(post => post.comments)
                .ThenInclude(comment => comment.author)
                .OrderByDescending(post => post.editDate)
                .Skip(offset)
                .Take(pageSize)
                .ToList();
        }

        public int getPageCount(int pageSize, int category)
        {
            List<int> userActivities = context.activities
                .Where(activity => activity.courseId == category)
                .Select(activity => activity.id)
                .ToList();
            return (context.posts
                .Where(post => post.courseId == category).Count() -1) / pageSize + 1;
        }
        public string save(Post post)
        {
            string result = "";
            try
            {
                context.posts.Add(post);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        public string update(Post post)
        {
            string result = "";
            try
            {
                context.posts.Update(post);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        public Post getPostWithComments(int id)
        {
            List<Post> posts = context.posts
                .Where(post => post.id == id)
                .Include(post => post.course)
                .Include(post => post.comments)
                .ThenInclude(comment => comment.editor)
                .Include(post => post.activity)
                .ThenInclude(activity => activity.course)
                .ToList();
            if (posts.Count != 1)
            {
                return null;
            }
            return posts[0];
        }

        public Post getPostByActivity(int activity)
        {
            List<Post> posts = context.posts
                .Where(post => post.activityId == activity)
                .ToList();
            if (posts.Count != 1)
            {
                return null;
            }
            return posts[0];
        }

        public PostComment GetComment(int id)
        {
            List<PostComment> comments = context.postComments
                .Where(comment => comment.id == id)
                .Include(comment => comment.post)
                .ToList();
            if (comments.Count != 1)
            {
                return null;
            }
            else
            {
                return comments[0];
            }
        }

        public string saveComment(PostComment comment)
        {
            string result = "";
            try
            {
                context.postComments.Add(comment);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public string updateComment(PostComment comment)
        {
            string result = "";
            try
            {
                context.postComments.Update(comment);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        public void deletePost(Post post)
        {
            context.posts.Remove(post);
            context.SaveChanges();
        }

        public void deleteComment(PostComment comment)
        {
            context.postComments.Remove(comment);
            context.SaveChanges();
        }

        internal List<CategoryModel> getCategories()
        {
            ApplicationUser currentUser = userService.getCurrentUser().Result;
            List<Course> userCourses = context.courseApplicationUsers.Where(cau => cau.ApplicationUserId == currentUser.Id && (cau.status == enCourseUserStatus.finished || cau.status == enCourseUserStatus.approved))
                .Include(cau => cau.course)
                .Select(cau => cau.course)
                .Distinct()
                .ToList();
            userCourses.Add(context.courses.Find(0));
            List<CategoryModel> result = new List<CategoryModel>(userCourses.Count);
            foreach(Course course in userCourses)
            {
                List<Post> post = context.posts
                    .Where(post => post.courseId == course.id)
                    .Include(post => post.comments)
                    .OrderByDescending(post => post.editDate)
                    .ToList();
                result.Add(new CategoryModel { categoryId = course.id, name =  course.name, newestActivity = post.Count > 0 ? post[0].editDate.ToString("dd-MM-yyyy") : "Never", posts = post.Count });
            }
            return result;
        }

        public void deletePostsByActivity(int id)
        {
            List<Post> posts = context.posts.Where(post => post.activityId == id).ToList();
            foreach (Post post in posts)
            {
                List<PostComment> comments = context.postComments.Where(comment => comment.postId == post.id).ToList();
                context.postComments.RemoveRange(comments);
            }
            context.SaveChanges();
            context.posts.RemoveRange(posts);
            context.SaveChanges();
        }

        public void deletePostsByCourse(int id)
        {
            List<Post> posts = context.posts.Where(post => post.courseId == id).ToList();
            foreach (Post post in posts)
            {
                List<PostComment> comments = context.postComments.Where(comment => comment.postId == post.id).ToList();
                context.postComments.RemoveRange(comments);
            }
            context.SaveChanges();
            context.posts.RemoveRange(posts);
            context.SaveChanges();
        }
    }
}
