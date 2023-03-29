using Microsoft.AspNetCore.Mvc;
using Thesis.Models;

namespace Thesis.ViewComponents
{
    [ViewComponent]
    public class CourseJoinStatusViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(enCourseUserStatus status)
        {
            string model = "";
            switch (status)
            {
                case enCourseUserStatus.waitingForApproval:
                    model = "Waiting for approval";
                    break;
                case enCourseUserStatus.approved:
                    model = "Joined the course";
                    break;
                case enCourseUserStatus.denied:
                    model = "Denied access";
                    break;
                case enCourseUserStatus.finished:
                    model = "Course finished";
                    break;
            }
            return await Task.FromResult((IViewComponentResult)View("CourseJoinStatus", model));
        }
    }
}
