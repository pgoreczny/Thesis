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
            List<int> userActivities = context.activities
                .Where(activity => activity.courseId == category)
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
            catch(Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        public Post getPostWithComments(int id)
        {
            List<Post> posts = context.posts
                .Where(post => post.id == id)
                .Include(post => post.comments)
                .Include(post => post.activity)
                .ThenInclude(activity => activity.course)
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
                .Where(comment => comment.id == 0)
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
            catch(Exception e)
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
        }

        public void deleteComment(PostComment comment)
        {
            context.postComments.Remove(comment);
        }

        internal List<string> getCategories(ApplicationUser result)
        {
            return context.courses.Select(course => course.name).ToList();
        }
    }
}
