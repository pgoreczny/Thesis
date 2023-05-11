using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Thesis.Areas.Identity.Constants;
using Thesis.Models;
using Thesis.Services;
using static Thesis.Areas.Identity.Constants.Claims;

namespace Thesis.Areas.Course.Controllers
{
    [Area("Course")]
    public class FeedController : Controller
    {
        private readonly CourseService courseService;
        private readonly ActivityService activityService;
        private readonly UserService userService;
        private readonly FileService fileService;
        private readonly AnswerService answerService;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public FeedController(CourseService courseService, ActivityService activityService, UserService userService, FileService fileService, AnswerService answerService)
        {
            this.courseService = courseService;
            this.activityService = activityService;
            this.userService = userService;
            this.answerService = answerService;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
            crumbs.Add(new Breadcrumb { text = "Available courses", url = "/course/feed/availableCourses" });
            this.fileService = fileService;
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
            if (checkAllowed(id))
            {
                Thesis.Models.Course course = courseService.getCourseById(id);
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
            if (!checkAllowed(id))
            {
                return LocalRedirect("/Home/showError");
            }
            Thesis.Models.Course course = courseService.getCourseById(id);
            return View("joinCourse", course);
        }
        [Authorize(Policy = Claims.UserCourses.JoinCourse)]
        public IActionResult join([FromQuery] int id)
        {
            ApplicationUser user = userService.getCurrentUser().Result;
            if (!checkAllowed(id))
            {
                return LocalRedirect("/Home/showError");
            }
            Thesis.Models.Course course = courseService.getCourseById(id);
            CourseApplicationUser courseUser = new CourseApplicationUser { course = course, CourseId = course.id, applicationUser = user, ApplicationUserId = user.Id, status = enCourseUserStatus.waitingForApproval };
            courseService.addUser(courseUser);
            return LocalRedirect("/Course/Feed/availableCourses");
        }
        [Authorize(Policy = Claims.UserCourses.JoinCourse)]
        public IActionResult leaveCourse(int id)
        {
            crumbs.Add(new Breadcrumb { text = "Leave course", current = true });
            ViewBag.crumbs = crumbs;
            if (!checkAllowed(id))
            {
                return LocalRedirect("/Home/showError");
            }
            Thesis.Models.Course course = courseService.getCourseById(id);
            return View("confirmLeave", course);
        }

        [Authorize(Policy = Claims.UserCourses.JoinCourse)]
        public IActionResult leave([FromQuery] int id)
        {
            ApplicationUser user = userService.getCurrentUser().Result;
            if (!checkAllowed(id))
            {
                return LocalRedirect("/Home/showError");
            }
            courseService.leaveCourse(user.Id, id);
            return LocalRedirect("/Course/Feed/availableCourses");
        }

        [Authorize(Policy = Claims.UserCourses.ParticipateInCourse)]
        public IActionResult feed([FromQuery] int id)
        {
            if (!checkAllowed(id))
            {
                return LocalRedirect("/Home/showError");
            }
            Thesis.Models.Course course = courseService.getCourseByIdWithDependencies(id);
            crumbs.Add(new Breadcrumb { text = course.name, current = true });
            ViewBag.crumbs = crumbs;
            return View("feed", course);
        }

        [Authorize(Policy = Claims.UserCourses.ParticipateInCourse)]
        public IActionResult AddAnswer(IFormFile file, [FromQuery] int activity)
        {
            if (!checkAllowedByActivity(activity))
            {
                return LocalRedirect("/Home/showError");
            }
            List<Answer> answers = answerService.findAnswer(activity, userService.getCurrentUser().Result.Id);
            Answer answer = answers.Count > 0 ? answers.Last() : null;
            if (answer == null)
            {
                answer = new Answer { entryDate = DateTime.Now, editable = false, isChecked = false, student = userService.getCurrentUser().Result, activityId = activity, version = 1 };
                answer = answerService.addAnswer(answer);
            }
            else
            {
                answer = new Answer { entryDate = DateTime.Now, editable = false, isChecked = false, student = userService.getCurrentUser().Result, activityId = activity, version = answer.version + 1 };
                answer = answerService.addAnswer(answer);
            }
            Thesis.Models.File fileModel = fileService.saveFile(file, enConnectionType.answer, answer.id);
            answerService.saveComment(new ReviewComment { answer = answer, AnswerId = answer.id, comment = "", position = "", wordNumber = 0, author = userService.getCurrentUser().Result });
            if (fileModel != null)
            {
                answerService.addFile(fileModel, answer.id);
            }
            return LocalRedirect("/Course/Feed/feed?id=" + activityService.getActivityById(activity).courseId);
        }

        [Authorize(Policy = Claims.UserCourses.ParticipateInCourse)]
        public OperationResult SaveAnswer([FromBody] string content, [FromQuery] int activity)
        {
            if (!checkAllowedByActivity(activity))
            {
                return new OperationResult { success = false, text = "You don't have access to this actvity" };
            }
            List<Answer> answers = answerService.findAnswer(activity, userService.getCurrentUser().Result.Id);
            Answer answer = answers.Count > 0 ? answers.Last() : null;
            string fileName = activityService.getActivityById(activity).title + "-" + userService.getCurrentUser().Result.UserName + ".html";
            if (answer == null)
            {
                answer = new Answer { entryDate = DateTime.Now, editable = true, isChecked = false, student = userService.getCurrentUser().Result, activityId = activity, version = 1 };
                answer = answerService.addAnswer(answer);
            }
            else
            {
                answer = new Answer { entryDate = DateTime.Now, editable = true, isChecked = false, student = userService.getCurrentUser().Result, activityId = activity, version = answer.version + 1 };
                answer = answerService.addAnswer(answer);
            }
            Thesis.Models.File fileModel = fileService.makeFile(answerService.parseAnswerText(content), fileName, enConnectionType.answer, answer.id);
            if (fileModel != null)
            {
                answerService.addFile(fileModel, answer.id);
            }
            return new OperationResult { success = true, text = "Answer saved successfully" };
        }

        [Authorize(Policy = Claims.UserCourses.ParticipateInCourse)]
        public IActionResult WriteAnswer([FromQuery] int activity)
        {
            if (!checkAllowedByActivity(activity))
            {
                return LocalRedirect("/Home/showError");
            }
            Models.Activity current = activityService.GetActivityByIdWithParent(activity);

            crumbs.Add(new Breadcrumb { text = current.course.name, url = "/course/feed/feed?id=" + current.courseId });
            crumbs.Add(new Breadcrumb { text = current.title, current = true });
            ViewBag.crumbs = crumbs;
            return View("AnswerEditor", (activity, current.courseId));
        }

        [Authorize(Policy = Claims.UserCourses.ParticipateInCourse)]
        public IActionResult comments([FromQuery] int id)
        {
            Answer answer = answerService.findAnswerWithParents(id);
            if (answer == null)
            {
                return LocalRedirect("/Home/showError");
            }
            if (!checkAllowedByActivity(answer.activityId))
            {
                return LocalRedirect("/Home/showError");
            }
            crumbs.Add(new Breadcrumb { text = answer.activity.course.name, url = "/course/feed/feed?id=" + answer.activity.course.id });
            crumbs.Add(new Breadcrumb { text = string.Format("{0} - Answer({1})", answer.activity.title, answer.version), current = true });
            ViewBag.crumbs = crumbs;
            Thesis.Models.File answerFile = fileService.getFileModel((Guid)answer.fileId);
            if (answer.editable)
            {
                byte[] file = fileService.getFile(answerFile);
                return View("commentedAnswer", (answer, System.Text.Encoding.Default.GetString(file), answer.comments));
            }
            else
            {
                return View("simpleCommentedAnswer", (answer, answerFile, answer.comments[0]));
            }
        }

        private bool checkAllowed(int id)
        {
            ApplicationUser user = userService.getCurrentUser().Result;
            return courseService.checkIfExists(id) || courseService.checkCourseAccess(user, id);
        }
        private bool checkAllowedByActivity(int id)
        {
            ApplicationUser user = userService.getCurrentUser().Result;
            Models.Activity activity = activityService.GetActivityByIdWithParent(id);
            if (activity == null)
            {
                return false;
            }
            return courseService.checkIfExists(activity.courseId) || courseService.checkCourseAccess(user, id);
        }
    }
}