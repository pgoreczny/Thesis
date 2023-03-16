﻿using Microsoft.EntityFrameworkCore;
using Thesis.database;
using Thesis.Models;
using File = Thesis.Models.File;

namespace Thesis.Services
{
    public class ActivityService
    {
        private readonly CoursesDBContext context;
        private readonly FileService fileService;
        public ActivityService(CoursesDBContext context, FileService fileService)
        {
            this.context = context;
            this.context.ChangeTracker.AutoDetectChangesEnabled = false;
            this.fileService = fileService;
        }
        public Activity getActivityById(int id)
        {
            List<Activity> activities = context.activities
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
            List<Activity> activities = context.activities.Where(activity => activity.id == id).ToList();
            context.activities.RemoveRange(activities);
            context.SaveChanges();
        }

        public void deleteActivities(List<int> ids)
        {
            List<Activity> activities = context.activities.Where(activity => ids.Contains(activity.id)).ToList();
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