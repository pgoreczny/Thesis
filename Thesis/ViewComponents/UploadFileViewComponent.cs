using Microsoft.AspNetCore.Mvc;

namespace Thesis.ViewComponents
{
    public class UploadFileViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync((string path, string, string) model)
        {
            if (string.IsNullOrEmpty(model.path))
            {
                model.path = "/Home/showError";
            }
            return await Task.FromResult((IViewComponentResult)View("UploadFile", model));
        }
    }
}
