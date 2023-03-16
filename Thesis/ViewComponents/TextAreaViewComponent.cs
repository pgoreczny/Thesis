using Microsoft.AspNetCore.Mvc;

namespace Thesis.ViewComponents
{
    public class TextAreaViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync((string name, string label, string value) model)
        {
            if (model.value == null)
            {
                model.value = "";
            }
            return await Task.FromResult((IViewComponentResult)View("TextArea", model));
        }
    }
}
