using Microsoft.AspNetCore.Mvc;

namespace Thesis.ViewComponents
{
    public class DateLabelViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(DateTime date)
        {
            return await Task.FromResult((IViewComponentResult)View("DateLabel", date));
        }
    }
}
