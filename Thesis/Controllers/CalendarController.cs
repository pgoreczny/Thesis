using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Thesis.Areas.Identity.Constants;
using Thesis.Models.Calendar;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.Controllers
{
    public class CalendarController : Controller
    {
        private readonly UserService userService;
        private readonly CourseService courseService;
        private readonly CalendarService calendarService;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public CalendarController(UserService userService, CalendarService calendarService, CourseService courseService)
        {
            this.calendarService = calendarService;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
            crumbs.Add(new Breadcrumb { text = "Calendar", url = "/calendar" });
            this.userService = userService;
            this.courseService = courseService;
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public ActionResult calendar([FromQuery] int year = 0, [FromQuery] int month = 0)
        {
            try
            {
                DateTime date = year == 0 || month == 0 ? DateTime.Now: new DateTime(year, month, 1);
                List<Week> calendar = calendarService.getCalendarModel(date);
                crumbs[1].current = true;
                return View("calendar", (calendar, date));
            }
            catch
            {
                return LocalRedirect("/Home/showError");
            }
        }

        [Authorize(Policy = Claims.UserCourses.ParticipateInCourse)]
        public ActionResult courseCalendar([FromQuery] int year, [FromQuery] int month, [FromQuery] int courseId)
        {
            try
            {
                DateTime date = year == 0 || month == 0 ? DateTime.Now : new DateTime(year, month, 1);
                if(!checkAllowedByCourse(courseId))
                {
                    return LocalRedirect("/Home/showError");
                }
                Course course = courseService.getCourseById(courseId);
                List<Week> calendar = calendarService.getCalendarModel(new DateTime(year, month, 1), courseId);
                crumbs.Add(new Breadcrumb { text = course.name, current = true });
                return View("courseCalendar", calendar);
            }
            catch
            {
                return LocalRedirect("/Home/showError");
            }
        }

        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        public ActionResult addCourseReminder([FromQuery] int courseId)
        {
            Course course = courseService.getCourseById(courseId);
            crumbs[1] = new Breadcrumb { text = "Courses", url = "/course/course/courselist" };
            crumbs.Add(new Breadcrumb { text = course.name, url = "/Course/Course/edit?id=\" + course.id" });
            crumbs.Add(new Breadcrumb { text = "Add reminder", current = true });
            return View("addReminder", courseId);
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public ActionResult addReminder([FromQuery] int day, [FromQuery] int month, [FromQuery] int year)
        {
            crumbs.Add(new Breadcrumb { text = "Add reminder", current = true });
            return View("addReminder", new Reminder { date = new DateTime(year, month, day)});
        }
        public ActionResult edit([FromQuery] int id)
        {
            crumbs.Add(new Breadcrumb { text = "Edit reminder", current = true });
            Reminder reminder = calendarService.getReminder(id);
            if (reminder == null || reminder.userId != userService.getCurrentUser().Result.Id)
            {
                return LocalRedirect("/Home/showError");
            }
            return View("addReminder", reminder);
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public OperationResult saveReminder([FromForm]Reminder reminder)
        {
            reminder.userId = userService.getCurrentUser().Result.Id;
            string result = calendarService.save(reminder);
            if (!new List<string> { "primary", "success", "danger", "warning", "secondary" }.Contains(reminder.color))
            {
                return new OperationResult { success = false, text = "Wrong value set for reminder color" };
            }
            if (result == "")
            {
                return new OperationResult { success = true, text = "Reminder saved successfully", data = new List<string> { string.Format("/calendar?year={0}&month={1}", reminder.date.Year, reminder.date.Month) } };
            }
            else
            {
                return new OperationResult { success = false, text = result };
            }
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public OperationResult deleteReminder([FromQuery] int id)
        {
            Reminder reminder = calendarService.getReminder(id);
            if (reminder == null)
            {
                return new OperationResult { success = false, text = "This reminder couldn't be found" };
            }
            bool canEditAny = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseEdit);
            if (canEditAny || reminder.userId == userService.getCurrentUser().Result.Id)
            {
                string result = calendarService.delete(reminder);
                if ( result == "")
                {
                    return new OperationResult { success = true, text = "Reminder deleted successfully"};
                }
                else
                {
                    return new OperationResult { success = false, text = result };
                }
            }
            else
            {
                return new OperationResult { success = false, text = "You can't delete this reminder" };
            }
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public ReminderInfo reminder([FromQuery] int id)
        {
            Reminder reminder = calendarService.getReminder(id);
            ApplicationUser user = userService.getCurrentUser().Result;
            if (reminder == null || !checkAllowedByCourse(reminder.courseId))
            {
                return new ReminderInfo { error = true, text = "The reminder doesn't exist or you don't have access to it."};
            }
            bool canModify = ((System.Security.Claims.ClaimsIdentity)User.Identity).HasClaim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseEdit);
            return new ReminderInfo(reminder, canModify);
        }

        private bool checkAllowedByCourse(int? id)
        {
            if (id < 1 || id == null)
            {
                return true;
            }
            ApplicationUser user = userService.getCurrentUser().Result;
            Thesis.Models.Course course = courseService.getCourseById((int)id);
            if (course == null)
            {
                return false;
            }
            return courseService.checkIfExists(id) && courseService.checkCourseAccess(user, (int)id);
        }
    }
}
