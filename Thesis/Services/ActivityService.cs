using Microsoft.EntityFrameworkCore;
using Thesis.database;
using Thesis.Models;
using File = Thesis.Models.File;

namespace Thesis.Services
{
    public class ActivityService
    {
        private readonly CoursesDBContext context;
        private readonly FileService fileService;
        private readonly ForumService forumService;
        public ActivityService(CoursesDBContext context, FileService fileService, ForumService forumService)
        {
            this.context = context;
            this.context.ChangeTracker.AutoDetectChangesEnabled = false;
            this.fileService = fileService;
            this.forumService = forumService;
        }

        public Activity GetActivityByIdWithParent(int id)
        {
            List<Activity> activities = context.activities
                .Include(activity => activity.course)
                .Where(activity => activity.id == id)
                .ToList();
            if (activities.Count == 0)
            {
                return null;
            }
            return activities[0];
        }

        public Activity getActivityById(int id)
        {
            List<Activity> activities = context.activities
                .Include(activity => activity.answers)
                .ThenInclude(answer => answer.student)
                .Where(activity => activity.id == id)
                .ToList();
            if (activities.Count == 0)
            {
                return new Activity();
            }
            activities[0].files = fileService.getFiles(enConnectionType.activity, activities[0].id);
            return activities[0];
        }

        public List<Activity> getActivities()
        {
            return context.activities
                .Where(activity => activity.id != 0)
                .ToList();
        }
        public List<Activity> getActivitiesByCourse(int courseId)
        {
            return context.activities
                .Where(activity => activity.id != 0 && activity.courseId == courseId)
                .ToList();
        }

        public int saveActivity(Activity activity)
        {
            context.Entry(getActivityById(activity.id)).State = EntityState.Detached;
            context.activities.Update(activity);
            context.SaveChanges();
            return activity.id;
        }

        public void deleteActivity(int id)
        {
            List<Activity> activities = context.activities.Where(activity => activity.id == id && activity.id != 0).ToList();
            forumService.deletePostsByActivity(id);
            context.activities.RemoveRange(activities);
            context.SaveChanges();
        }

        public void deleteActivitiesByCourse(int id)
        {
            List<Activity> activities = context.activities.Where(activity => activity.courseId == id).ToList();
            foreach(Activity activity in activities)
            {
                deleteActivity(activity.id);
            }
        }

        public void deleteActivities(List<int> ids)
        {
            List<Activity> activities = context.activities.Where(activity => ids.Contains(activity.id) && activity.id != 0).ToList();
            context.activities.RemoveRange(activities);
            context.SaveChanges();
        }

        public bool checkIfExists(int id)
        {
            return context.activities.Any(course => course.id == id);
        }

        public bool checkIfExists(List<int> ids)
        {
            foreach (int id in ids)
            {
                if (!checkIfExists(id))
                {
                    return false;
                }
            }
            return true;
        }

        public bool addFile(File file, int activityId)
        {
            Activity? activity = context.activities.Find(activityId);
            if (activity != null)
            {
                activity.files.Add(file);
                return true;
            }
            return false;
        }
    }
}