using Microsoft.EntityFrameworkCore;
using Thesis.database;
using Thesis.Models;

namespace Thesis.Services
{
    public class CourseService
    {
        private readonly CoursesDBContext context;
        public CourseService(CoursesDBContext context)
        {
            this.context = context;
        }

        public Course getCourseById(int id)
        {
            List<Course> courses = context.courses
                .Include(course => course.activities)
                .Include(course => course.CourseApplicationUsers)
                .Where(course => course.id == id).ToList();
            if (courses.Count == 0)
            {
                return new Course();
            }
            return courses[0];
        }

        public List<Course> getCourses(bool withChildren = false)
        {
            if (withChildren)
            {
                return context.courses
                    .Include(course => course.createdBy)
                    .Include(course => course.updatedBy)
                    .Include(course => course.activities)
                    .Include(course => course.users)
                    .ToList();
            }
            else
            {
                return context.courses
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
            List<Course> courses = context.courses.Where(course => ids.Contains(course.id)).ToList();
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
    }
}