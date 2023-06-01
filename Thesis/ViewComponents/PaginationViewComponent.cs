using Microsoft.AspNetCore.Mvc;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.ViewComponents
{
    public class PaginationViewComponent: ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Pagination pagination)
        {

            return await Task.FromResult((IViewComponentResult)View("Pagination", pagination));
        }
    }
}
