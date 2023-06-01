using Microsoft.AspNetCore.Mvc;

namespace Thesis.ViewComponents
{
    public class DateWithTextViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync((DateTime date, string text) model)
        {
            return await Task.FromResult((IViewComponentResult)View("DateWithText", model));
        }
    }
}
