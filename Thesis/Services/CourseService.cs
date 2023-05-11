using Microsoft.EntityFrameworkCore;
using Thesis.database;
using Thesis.Models;

namespace Thesis.Services
{
    public class CourseService
    {
        private readonly CoursesDBContext context;
        private readonly UserService userService;
        private readonly FileService fileService;
        public CourseService(CoursesDBContext context, UserService userService, FileService fileService)
        {
            this.context = context;
            this.userService = userService;
            this.fileService = fileService;
        }

        public void addUser(CourseApplicationUser user)
        {
            context.courseApplicationUsers.Add(user);
            context.SaveChanges();
        }

        public void leaveCourse(string userId, int courseId)
        {
            context.courseApplicationUsers.Remove(getJoin(courseId, userId));
            context.SaveChanges();
        }

        public CourseApplicationUser getJoin(int courseId, string userId)
        {
            return context.courseApplicationUsers
                .Where(join => join.CourseId == courseId && join.ApplicationUserId == userId)
                .First();
        }

        public string updateConnection(int courseId, string userId, enCourseUserStatus status)
        {
            try
            {
                CourseApplicationUser join = getJoin(courseId, userId);
                join.status = status;
                context.courseApplicationUsers.Update(join);
                context.SaveChanges();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return "";
        }

        public Course getCourseById(int id)
        {
            List<Course> courses = context.courses
                .Include(course => course.activities)
                .Include(course => course.CourseApplicationUsers)
                    .ThenInclude(join => join.applicationUser)
                .Where(course => course.id == id && course.id != 0).ToList();
            if (courses.Count == 0)
            {
                return new Course();
            }
            return courses[0];
        }

        public Course getCourseByIdWithDependencies(int id)
        {
            List<Course> courses = context.courses
                .Include(course => course.activities)
                    .ThenInclude(activity => activity.answers)
                .Include(course => course.CourseApplicationUsers)
                    .ThenInclude(join => join.applicationUser)
                .Where(course => course.id == id && course.id != 0).ToList();
            if (courses.Count == 0)
            {
                return new Course();
            }
            foreach (Activity activity in courses[0].activities)
            {
                activity.files = fileService.getFiles(enConnectionType.activity, activity.id);
            }
            return courses[0];
        }

        public List<Course> getAvailable()
        {
            return context.courses
                .Where(course => course.id != 0 && course.startDate.CompareTo(DateTime.Now) < 0 && course.endDate.CompareTo(DateTime.Now) > 0)
                .ToList();
        }



        public List<Course> getCourses(bool withChildren = false)
        {
            if (withChildren)
            {
                return context.courses
                    .Where(course => course.id != 0)
                    .Include(course => course.createdBy)
                    .Include(course => course.updatedBy)
                    .Include(course => course.activities)
                    .Include(course => course.users)
                    .ToList();
            }
            else
            {
                return context.courses
                    .Where(course => course.id != 0)
                    .Include(course => course.createdBy)
                    .Include(course => course.updatedBy)
                    .ToList();
            }
        }

        public void saveCourse(Course course)
        {
            context.courses.Update(course);
            context.SaveChanges();
        }

        public void deleteCourses(List<int> ids)
        {
            List<Course> courses = context.courses.Where(course => ids.Contains(course.id) && course.id != 0).ToList();
            context.courses.RemoveRange(courses);
            context.SaveChanges();
        }

        public bool checkIfExists(int id)
        {
            return context.courses.Any(course => course.id == id);
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


        public List<CourseApplicationUser> getUserCourses(string userId)
        {
            return context.Users
                .Include(x => x.CourseApplicationUsers)
                .Where(x => x.Id == userId)
                .First()
                .CourseApplicationUsers;
        }

        public bool checkCourseAccess(ApplicationUser user, int courseId)
        {
            List<CourseApplicationUser> joins = context.courseApplicationUsers
                .Where(join => join.CourseId == courseId && join.ApplicationUserId == user.Id && (join.status == enCourseUserStatus.approved || join.status == enCourseUserStatus.finished))
                .ToList();
            return joins.Count > 0;
        }
    }
}