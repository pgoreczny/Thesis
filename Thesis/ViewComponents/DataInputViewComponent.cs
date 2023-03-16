using Microsoft.AspNetCore.Mvc;

namespace Thesis.ViewComponents
{
    public class DateInputViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync((string name, string label, DateTime value) model)
        {
            return await Task.FromResult((IViewComponentResult)View("DateInput", model));
        }
    }
}
