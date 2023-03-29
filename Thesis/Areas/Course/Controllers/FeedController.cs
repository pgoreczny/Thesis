using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.Areas.Identity.Constants;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.Areas.Course.Controllers
{
    [Area("Course")]
    public class FeedController : Controller
    {
        private readonly CourseService courseService;
        private readonly ActivityService activityService;
        private readonly UserService userService;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public FeedController(CourseService courseService, ActivityService activityService, UserService userService)
        {
            this.courseService = courseService;
            this.activityService = activityService;
            this.userService = userService;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
            crumbs.Add(new Breadcrumb { text = "Available courses", url = "/course/feed/availableCourses" });
        }

        [Authorize(Policy = Claims.UserCourses.SeeCourses)]
        public IActionResult availableCourses()
        {
            crumbs[1] = new Breadcrumb { text = "Available courses", current = true };
            ViewBag.crumbs = crumbs;
            List<CourseApplicationUser> joins = courseService.getUserCourses(userService.getCurrentUser().Result.Id);
            return View("courses", (courseService.getAvailable(), joins));
        }

        [Authorize(Policy = Claims.UserCourses.AccessCourse)]
        public IActionResult course(int id)
        {
            if (courseService.checkCourseAccess(userService.getCurrentUser().Result, id))
            {
                Models.Course course = courseService.getCourseById(id);
                crumbs.Add(new Breadcrumb { text = course.name, current = true });
                ViewBag.crumbs = crumbs;
                return View("courses", course);
            }
            else
            {
                return LocalRedirect("/Course/Feed/joinCourse?id=" + id);
            }
        }
        [Authorize(Policy = Claims.UserCourses.JoinCourse)]
        public IActionResult joinCourse(int id)
        {
            crumbs.Add(new Breadcrumb { text = "Join the course", current = true });
            ViewBag.crumbs = crumbs;
            ApplicationUser user = userService.getCurrentUser().Result;
            if (!courseService.checkIfExists(id) || courseService.checkCourseAccess(user, id))
            {
                return LocalRedirect("/Home/showError");
            }
            Models.Course course = courseService.getCourseById(id);
            return View("joinCourse", course);
        }
        [Authorize(Policy = Claims.UserCourses.JoinCourse)]
        public IActionResult join([FromQuery] int id)
        {
            ApplicationUser user = userService.getCurrentUser().Result;
            if (!courseService.checkIfExists(id) || courseService.checkCourseAccess(user, id))
            {
                return LocalRedirect("/Home/showError");
            }
            Models.Course course = courseService.getCourseById(id);
            CourseApplicationUser courseUser = new CourseApplicationUser { course = course, CourseId = course.id, applicationUser = user, ApplicationUserId = user.Id, status = enCourseUserStatus.waitingForApproval };
            courseService.addUser(courseUser);
            return LocalRedirect("/Course/Feed/availableCourses");
        }
        [Authorize(Policy = Claims.UserCourses.JoinCourse)]
        public IActionResult leaveCourse(int id)
        {
            crumbs.Add(new Breadcrumb { text = "Leave course", current = true });
            ViewBag.crumbs = crumbs;
            ApplicationUser user = userService.getCurrentUser().Result;
            if (!courseService.checkIfExists(id) || !courseService.checkCourseAccess(user, id))
            {
                return LocalRedirect("/Home/showError");
            }
            Models.Course course = courseService.getCourseById(id);
            return View("confirmLeave", course);
        }

        [Authorize(Policy = Claims.UserCourses.JoinCourse)]
        public IActionResult leave([FromQuery] int id)
        {
            ApplicationUser user = userService.getCurrentUser().Result;
            if (!courseService.checkIfExists(id) || !courseService.checkCourseAccess(user, id))
            {
                return LocalRedirect("/Home/showError");
            }
            courseService.leaveCourse(user.Id, id);
            return LocalRedirect("/Course/Feed/availableCourses");
        }
    }
}
