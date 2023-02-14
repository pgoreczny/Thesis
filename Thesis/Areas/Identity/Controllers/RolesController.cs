using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using Thesis.Areas.Identity.Constants;
using Thesis.Models;
using System.Security.Claims;
using Thesis.Areas.Identity.Models;
using Roles = Thesis.Areas.Identity.Constants.Roles;
using static Thesis.Areas.Identity.Constants.Claims;

namespace Thesis.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task setUp()
        {
            IdentityRole admin = new IdentityRole(Roles.Admin);
            IdentityRole lecturer = new IdentityRole(Roles.Lecturer);
            IdentityRole student = new IdentityRole(Roles.Student);
            IdentityRole registered = new IdentityRole(Roles.Registered);
            await roleManager.CreateAsync(admin);
            await roleManager.CreateAsync(lecturer);
            await roleManager.CreateAsync(student);
            await roleManager.CreateAsync(registered);
            admin = await roleManager.FindByNameAsync(Roles.Admin);
            lecturer = await roleManager.FindByNameAsync(Roles.Lecturer);
            student = await roleManager.FindByNameAsync(Roles.Student);
            registered = await roleManager.FindByNameAsync(Roles.Registered);
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Roles.AssignRole));
            await roleManager.AddClaimAsync(registered, new Claim(CustomClaimTypes.Permission, Claims.Basic.IsRegistered));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Users.UserList));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Users.UserAdd));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Users.UserEdit));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Users.UserDelete));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.Users.UserList));
        }

        [Authorize(Policy = Claims.Roles.AssignRole)]
        public IActionResult AssignRole()
        {
            List<IdentityUser> identityUsers = userManager.Users.ToList();
            List<UserModel> users = new List<UserModel>(identityUsers.Count);
            foreach (IdentityUser user in identityUsers)
            {
                users.Add(new UserModel { userName = user.UserName, roles = userManager.GetRolesAsync(user).Result.ToList() });
            }
            return View("RolesAssignment", users);
        }

        [HttpPost]
        [Authorize(Policy = Claims.Roles.AssignRole)]
        public async Task<OperationResult> AssignRole([FromBody] Role role)
        {
            IdentityUser user = await userManager.FindByIdAsync(role.userId);

            if (user != null && Roles.list.Contains(role.name))
            {
                IdentityResult result = role.isOn ? await userManager.AddToRoleAsync(user, role.name) : await userManager.RemoveFromRoleAsync(user, role.name);
                if (result.Succeeded)
                {
                    return new OperationResult { success = true, text = "Role set successfully" };
                }
                return new OperationResult { success = false, text = result.Errors.First().Description };
            }
            else
            {
                return new OperationResult { success = false, text = "Role couldn't be found. Try again." };
            }
        }
    }
}
