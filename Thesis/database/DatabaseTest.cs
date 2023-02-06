using Thesis.Models;

namespace Thesis.database
{
    public class DatabaseTest
    {
        public static void addMenus()
        {
            using (var context = new CoursesDBContext())
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
        }
        public static List<MenuItem> getMenus()
        {
            List<MenuItem> menu = new List<MenuItem>();
            using (var context = new CoursesDBContext())
            {
                menu = context.menus
                    .Where(menu => menu.Parent == null)
                    .ToList();
            }
            return menu;
        }
    }
}