using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using Thesis.database;
using Thesis.Models;
using Thesis.Services;
using Activity = System.Diagnostics.Activity;

namespace Thesis.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService userService;
        private readonly MenuService menuService;
        private readonly CoursesDBContext context;

        public HomeController(ILogger<HomeController> logger, CoursesDBContext context, UserService userService, MenuService menuService)
        {
            _logger = logger;
            this.context = context;
            this.userService = userService;
            this.menuService = menuService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult saveMenus()
        {
            menuService.addMenus();
            return View("index");
        }

        [Authorize]
        public async Task<IActionResult> confidentialAsync()
        {
            return View("confidential");
        }
    }
}