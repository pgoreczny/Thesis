﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Thesis.Areas.Identity.Constants;
using Thesis.Migrations;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.Areas.Course.Controllers
{
    [Area("Course")]
    [Authorize]
    public class CourseController : Controller
    {
        private readonly CourseService courseService;
        private readonly ActivityService activityService;
        private readonly UserService userService;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public CourseController(CourseService courseService, ActivityService activityService, UserService userService)
        {
            this.courseService = courseService;
            this.activityService = activityService;
            this.userService = userService;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
            crumbs.Add(new Breadcrumb { text = "Courses", url = "/course/course/courselist" });
        }
        [Authorize(Policy = Claims.ManageCourses.CourseList)]
        public IActionResult CourseList()
        {
            List<Models.Course> courses = courseService.getCourses().ToList();
            crumbs[1] = new Breadcrumb { text = "Courses", current = true };
            ViewBag.crumbs = crumbs;
            return View("courseList", courses);
        }
        [Authorize(Policy = Claims.ManageCourses.CourseAdd)]
        public IActionResult add()
        {
            crumbs.Add(new Breadcrumb { text = "Add course", current = true });
            ViewBag.crumbs = crumbs;
            return View("CourseAdd", new Models.Course());
        }
        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        public IActionResult edit([FromQuery(Name = "id")] int id)
        {
            crumbs.Add(new Breadcrumb { text = "Edit course", current = true });
            ViewBag.crumbs = crumbs;
            Models.Course course = courseService.getCourseById(id);
            List<Activity> activities = activityService.getActivities();
            return View("CourseEdit", (course, activities));
        }
        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        [HttpPost]
        public IActionResult save([FromForm] Models.Course course)
        {
            if (course.createdBy == null)
            {
                course.createdBy = userService.getCurrentUser().Result;
                course.createDate = DateTime.Now;
            }
            course.updatedBy = userService.getCurrentUser().Result;
            course.updateDate = DateTime.Now;
            courseService.saveCourse(course);
            return LocalRedirect("/Course/Course/edit?id=" + course.id);
        }

        [Authorize(Policy = Claims.ManageCourses.CourseDelete)]
        [HttpDelete]
        public OperationResult delete([FromBody] List<int> ids)
        {
            if (courseService.checkIfExists(ids))
            {
                courseService.deleteCourses(ids);
                return new OperationResult { success = true, text = ids.Count > 1 ? "Courses deleted successfully" : "Course deleted successfully" };
            }
            return new OperationResult { success = false, text = "One of the courses couldn't be found" };
        }

        [Authorize(Policy = Claims.ManageCourses.ManageUsers)]
        [HttpPost]
        public OperationResult approveUser([FromQuery] string id, [FromQuery] int courseId)
        {
            ApplicationUser user = userService.getUserById(id).Result;
            if (user != null)
            {
                user.CourseApplicationUsers.Find(course => course.CourseId == courseId).status = enCourseUserStatus.approved;
                IdentityResult result = userService.updateUser(user);
                return new OperationResult { success = result.Succeeded, text = result.Succeeded ? "User approved" : result.Errors.FirstOrDefault().Description };
            }
            return new OperationResult { success = false, text = "User couldn't be found" };
        }

        [Authorize(Policy = Claims.ManageCourses.ManageUsers)]
        [HttpPost]
        public OperationResult denyUser([FromQuery] string id, [FromQuery] int courseId)
        {
            ApplicationUser user = userService.getUserById(id).Result;
            if (user != null)
            {
                user.CourseApplicationUsers.Find(course => course.CourseId == courseId).status = enCourseUserStatus.denied;
                IdentityResult result = userService.updateUser(user);
                return new OperationResult { success = result.Succeeded, text = result.Succeeded ? "User approved" : result.Errors.FirstOrDefault().Description };
            }
            return new OperationResult { success = false, text = "User couldn't be found" };
        }
    }
}