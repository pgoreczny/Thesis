using Microsoft.AspNetCore.Mvc;
using Thesis.database;
using Thesis.Models;

namespace Thesis.ViewComponents
{
    public class MenuViewComponent: ViewComponent
    {
        List<MenuItem>menu = new List<MenuItem>();
        public MenuViewComponent()
        {
            menu = DatabaseTest.getMenus();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = menu;
            return await Task.FromResult((IViewComponentResult)View("Menu", model));
        }
    }
}
