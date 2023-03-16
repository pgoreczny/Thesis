using Microsoft.AspNetCore.Mvc;

namespace Thesis.ViewComponents
{
    public class CheckboxViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync((string name, string label, bool value) model)
        {
            if (model.value == null)
            {
                model.value = false;
            }
            return await Task.FromResult((IViewComponentResult)View("Checkbox", model));
        }
    }
}
