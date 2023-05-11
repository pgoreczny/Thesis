using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thesis.Areas.Identity.Constants;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.Areas.Course.Controllers
{
    [Area("Course")]
    public class GradingController : Controller
    {
        private readonly CourseService courseService;
        private readonly ActivityService activityService;
        private readonly UserService userService;
        private readonly FileService fileService;
        private readonly AnswerService answerService;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public GradingController(CourseService courseService, ActivityService activityService, UserService userService, FileService fileService, AnswerService answerService)
        {
            this.courseService = courseService;
            this.activityService = activityService;
            this.userService = userService;
            this.answerService = answerService;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
            crumbs.Add(new Breadcrumb { text = "Courses", url = "/Course/Course/Courselist" });
            this.fileService = fileService;
        }

        [Authorize(Policy = Claims.ManageCourses.ManageUsers)]
        public IActionResult grade(int answerId)
        {
            Answer answer = answerService.findAnswerWithParents(answerId);
            crumbs.Add(new Breadcrumb { text = answer.activity.course.name, url = "/Course/Course/edit?id=" + answer.activity.course.id });
            crumbs.Add(new Breadcrumb { text = answer.activity.title, url = "/Course/Activity/edit?id=" + answer.activity.id });
            crumbs.Add(new Breadcrumb { text = answer.student.UserName, current = true });
            ViewBag.crumbs = crumbs;
            Thesis.Models.File answerFile = fileService.getFileModel((Guid)answer.fileId);
            if (answer.editable)
            {
                byte[] file = fileService.getFile(answerFile);
                return View("grade", (answer, System.Text.Encoding.Default.GetString(file), answer.comments));
            }
            else
            {
                return View("simpleGrade", (answer, answerFile, answer.comments[0]));
            }
        }

        [Authorize(Policy = Claims.ManageCourses.ManageUsers)]
        [HttpPost]
        public OperationResult comment([FromBody] ReviewComment comment)
        {
            OperationResult result;
            comment.author = userService.getCurrentUser().Result;
            comment.wordNumber = int.Parse(comment.position.Replace("word", ""));
            comment.answer = answerService.findAnswerWithParents(comment.AnswerId);
            if (comment.id == 0)
            {
                string error = answerService.saveComment(comment);
                if (string.IsNullOrEmpty(error))
                {
                    result = new OperationResult { success = true, text = "Comment added successfully", data = new List<string> { comment.id.ToString(), comment.author.UserName } };
                }
                else
                {
                    result = new OperationResult { success = false, text = error };
                }

            }
            else
            {
                string error = answerService.updateComment(comment);
                if (string.IsNullOrEmpty(error))
                {
                    result = new OperationResult { success = true, text = "Comment updated successfully", data = new List<string> { comment.id.ToString(), comment.author.UserName } };
                }
                else
                {
                    result = new OperationResult { success = false, text = error, data = new List<string> { comment.id.ToString(), comment.author.UserName } };
                }
            }
            return result;
        }
        [Authorize(Policy = Claims.ManageCourses.ManageUsers)]
        [HttpDelete]
        public OperationResult deleteComment([FromQuery] int comment)
        {
            if (answerService.deleteComment(comment))
            {
                return new OperationResult { success = true, text = "Comment deleted successfully" };
            }
            else
            {
                return new OperationResult { success = false, text = "Comment not found" };
            }
        }
        [Authorize(Policy = Claims.ManageCourses.ManageUsers)]
        public IActionResult submitReview([FromQuery] int answerId)
        {
            Answer answer = answerService.findAnswerWithParents(answerId);
            answer.isChecked = true;
            answerService.updateAnswer(answer);
            return LocalRedirect("/course/Activity/edit?id=" + answer.activityId);
        }
        [Authorize(Policy = Claims.ManageCourses.ManageUsers)]
        [HttpPost]
        public OperationResult saveSimpleComment([FromQuery] int id, [FromBody] string content)
        {
            ReviewComment comment = answerService.findComment(id);
            if (comment == null)
            {
                return new OperationResult { success = false, text = "Comment couldn't be found" };
            }
            comment.comment = content;
            comment.author = userService.getCurrentUser().Result;
            answerService.updateComment(comment);
            return new OperationResult { success = true, text = "Comment saved successfully" };
        }
    }
}
