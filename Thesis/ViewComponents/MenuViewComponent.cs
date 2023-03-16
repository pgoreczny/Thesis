using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Thesis.database;
using Thesis.Models;
using Thesis.Services;
using Task = System.Threading.Tasks.Task;

namespace Thesis.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly MenuService menuService;
        public MenuViewComponent(MenuService menuService)
        {
            this.menuService = menuService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await menuService.getMenus();
            return await Task.FromResult((IViewComponentResult)View("Menu", model));
        }
    }
}
