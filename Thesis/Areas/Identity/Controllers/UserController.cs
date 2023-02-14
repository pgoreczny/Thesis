using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Thesis.Areas.Identity.Constants;
using Thesis.Areas.Identity.Models;
using Thesis.Models;
using Thesis.Services;

namespace Thesis.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserService userService;
        public UserController(UserManager<IdentityUser> userManager, UserService userService, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.roleManager = roleManager;
        }

        [Authorize(Policy = Claims.Users.UserList)]
        public IActionResult UserList()
        {
            List<IdentityUser> identityUsers = userManager.Users.ToList();
            List<UserModel> users = new List<UserModel>(identityUsers.Count);
            foreach (IdentityUser user in identityUsers)
            {
                users.Add(userService.GetUserModel(user));
            }
            return View("UserList", users);
        }
        [Authorize(Policy = Claims.Users.UserEdit)]
        public IActionResult editUser([FromQuery(Name = "id")] string id)
        {
            UserModel user = userService.GetUserModel(userManager.Users.Where(user => user.Id == id).FirstOrDefault());
            IList<string> userroles = userService.getUserRoles().Result;
            List<Role> roles = roleManager.Roles.Select(role => new Role { name = role.Name, isOn = userroles.Contains(role.Name) }).ToList();
            return View("UserEdit", (user, roles));
        }

        public OperationResult save([FromForm] UserModel userModel)
        {
            IdentityUser user = userService.getUserById(userModel.id).Result;
            if (user != null)
            {
                user.Email = userModel.email;
                user.UserName = userModel.userName;
                IdentityResult result = userManager.UpdateAsync(user).Result;
                if (result.Succeeded)
                {
                    return new OperationResult { success = true, text = "User updated successfully" };
                }
                else
                {
                    return new OperationResult(result);
                }
            }
            return new OperationResult { success = false, text = "User couldn't be found" };
        }
    }
}
