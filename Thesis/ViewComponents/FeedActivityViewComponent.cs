using Microsoft.AspNetCore.Mvc;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.ViewComponents
{
    public class FeedActivityViewComponent: ViewComponent
    {
        private readonly AnswerService answerService;
        private readonly UserService userService;
        public FeedActivityViewComponent(AnswerService answerService, UserService userService)
        {
            this.answerService = answerService;
            this.userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync(Activity model)
        {
            Answer answer = answerService.findAnswer(model.id, userService.getCurrentUser().Result.Id);
            return await Task.FromResult((IViewComponentResult)View("FeedActivity", (model, answer)));
        }
    }
}
