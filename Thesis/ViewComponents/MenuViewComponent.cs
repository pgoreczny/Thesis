using Microsoft.AspNetCore.Mvc;
using Thesis.Models;

namespace Thesis.ViewComponents
{
    public class MenuViewComponent: ViewComponent
    {
        List<MenuItem>menu = new List<MenuItem>();
        public MenuViewComponent()
        {
            MenuItem child = new MenuItem { name = "Submenu test", url = "/Home/Privacy" };
            menu.Add(new MenuItem { Id = 1, name = "Test 1", url = "/Text/TextEditor" });
            menu.Add(new MenuItem { Id = 2, name = "Test 2", url = "/File/Index" });
            menu.Add(new MenuItem { Id = 3, name = "Test 3", url = "/" });
            menu.Add(new MenuItem { Id = 4, name = "Test 4", url = "/" });
            menu[3].children.Add(child);
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = menu;
            return await Task.FromResult((IViewComponentResult)View("Menu", model));
        }
    }
}
