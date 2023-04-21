using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.Areas.Identity.Constants;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.Areas.Course.Controllers
{
    [Area("Course")]
    public class ActivityController : Controller
    {
        private readonly CourseService courseService;
        private readonly ActivityService activityService;
        private readonly FileService fileService;
        private readonly UserService userService;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public ActivityController(CourseService courseService, ActivityService activityService, FileService fileService, UserService userService)
        {
            this.courseService = courseService;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
            this.activityService = activityService;
            this.fileService = fileService;
            this.userService = userService;
        }
        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        public IActionResult activitiesByCourse(int courseId)
        {
            List<Activity> activities = courseService.getCourseById(courseId).activities;
            return View("ActivityListPartial", activities);
        }

        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        public IActionResult add([FromQuery]int courseId, [FromQuery] string type)
        {
            crumbs.Add(new Breadcrumb { text = "Courses", url = "/Course/Course/CourseList" });
            crumbs.Add(new Breadcrumb { text = "Course", url = "/Course/Course/edit?id=" + courseId });
            crumbs.Add(new Breadcrumb { text = "Add activity", current = true });
            ViewBag.crumbs = crumbs;
            Activity activity;
            if (type == "task")
            {
                activity = new Activity(enActivityType.task);
            }
            else
            {
                activity = new Activity();
            }
            activity.courseId = courseId;
            return View("ActivityEdit", activity);
        }


        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        public IActionResult edit([FromQuery] int id)
        {
            Activity activity = activityService.getActivityById(id);
            crumbs.Add(new Breadcrumb { text = "Courses", url = "/Course/Course/CourseList" });
            crumbs.Add(new Breadcrumb { text = "Course", url = "/Course/Course/edit?id=" + activity.courseId});
            crumbs.Add(new Breadcrumb { text = "Activity", current = true });
            ViewBag.crumbs = crumbs;
            return View("ActivityEdit", activity);
        }

        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        [HttpPost]
        public IActionResult save([FromForm] Activity activity)
        {
            if (activity.createdBy == null)
            {
                activity.createdBy = userService.getCurrentUser().Result;
                activity.createDate = DateTime.Now;
            }
            activity.updatedBy = userService.getCurrentUser().Result;
            activity.updateDate = DateTime.Now;
            activity.course = courseService.getCourseById(activity.courseId);
            int id  = activityService.saveActivity(activity);
            return LocalRedirect("/course/Activity/edit?id=" + id);
        }

        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        public IActionResult saveFile(IFormFile file, [FromQuery] int id)
        {
            Models.File fileModel = fileService.saveFile(file, enConnectionType.activity, id);
            if (fileModel != null)
            {
                activityService.addFile(fileModel, id);
            }
            return LocalRedirect("/Course/Activity/edit?id=" + id);
        }

        [Authorize(Policy = Claims.UserCourses.ParticipateInCourse)]
        public FileResult getFile(Guid id)
        {
            Models.File file = fileService.getFileModel(id);
            FileContentResult result = new FileContentResult(fileService.getFile(file), fileService.getFileType(file))
            {
                FileDownloadName = file.showName
            };

            return result;
        }

        [Authorize(Policy = Claims.ManageCourses.CourseEdit)]
        [HttpDelete]
        public OperationResult delete([FromQuery] int id)
        {
            if (activityService.checkIfExists(id))
            {
                activityService.deleteActivity(id);
                return new OperationResult { success = true, text = "Activity deleted successfully" };
            }
            else
            {
                return new OperationResult { success = false, text = "Activity couldn't be found" };
            }
        }
    }
}
