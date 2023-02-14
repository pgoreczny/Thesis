using Microsoft.AspNetCore.Identity;
using Thesis.database;
using Thesis.Models;

namespace Thesis.Services
{
    public class MenuService
    {
        private readonly CoursesDBContext context = new CoursesDBContext();
        private readonly UserService userService;

        public MenuService(CoursesDBContext context, UserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public void addMenus()
        {
            List<MenuItem> menu = new List<MenuItem>();
            MenuItem child = new MenuItem { name = "Submenu test", url = "/Home/Privacy" };
            menu.Add(new MenuItem { name = "Test 1", url = "/Text/TextEditor" });
            menu.Add(new MenuItem { name = "Test 2", url = "/File/Index" });
            menu.Add(new MenuItem { name = "Test 3", url = "/" });
            menu.Add(new MenuItem { name = "Test 4", url = "/", children = new List<MenuItem>() });
            menu[3].children.Add(child);
            context.AddRange(menu);
            context.SaveChanges();
        }
        public async Task<List<MenuItem>> getMenus()
        {
            List<MenuItem> menu = new List<MenuItem>();
            IList<string> claims = await userService.getUserClaims();
            menu = context.menus
                .Where(menu => menu.Parent == null && claims.Contains(menu.claim))
                .ToList();
            return menu;
        }
    }
}