using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
        private readonly SignInManager<IdentityUser> signInManager;
        public UserController(UserManager<IdentityUser> userManager, UserService userService, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
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
            IdentityUser identityUser = userManager.Users.Where(user => user.Id == id).FirstOrDefault();
            UserModel user = userService.GetUserModel(identityUser);
            IList<string> userroles = userManager.GetRolesAsync(identityUser).Result;
            List<Role> roles = roleManager.Roles.Select(role => new Role { name = role.Name, isOn = userroles.Contains(role.Name) }).ToList();
            return View("UserEdit", (user, roles));
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public IActionResult myProfile()
        {
            IdentityUser identityUser = userService.getCurrentUser().Result;
            UserModel user = userService.GetUserModel(identityUser);
            IList<string> userroles = userManager.GetRolesAsync(identityUser).Result;
            List<Role> roles = roleManager.Roles.Select(role => new Role { name = role.Name, isOn = userroles.Contains(role.Name) }).ToList();
            return View("userProfile", (user, roles));
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public IActionResult getUserInfo([FromQuery(Name = "id")] string id)
        {
            IdentityUser identityUser = userManager.Users.Where(user => user.Id == id).FirstOrDefault();
            UserModel user = userService.GetUserModel(identityUser);
            return View("UserInfo", user);
        }

        [Authorize(Policy = Claims.Users.UserEdit)]
        [HttpPost]
        public async Task<IActionResult> save([FromForm] UserModel userModel)
        {
            IdentityUser user = userService.getUserById(userModel.id).Result;
            if (user != null)
            {
                user.Email = userModel.email;
                user.UserName = userModel.userName;
                IdentityResult result = userManager.UpdateAsync(user).Result;
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                }
            }
            return LocalRedirect("/Identity/User/editUser?id=" + userModel.id);
        }

        public async Task<IActionResult> saveProfile([FromForm] UserModel userModel)
        {
            IdentityUser user = userService.getUserById(userModel.id).Result;
            if (user != null)
            {
                user.Email = userModel.email;
                user.UserName = userModel.userName;
                IdentityResult result = userManager.UpdateAsync(user).Result;
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                }
            }
            return LocalRedirect("/Identity/User/myProfile");
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        [HttpPost]
        public OperationResult changePassword([FromBody]PasswordModel data)
        {
            IdentityUser user = userService.getUserById(data.userId).Result;
            if (user != null && User.FindFirstValue(ClaimTypes.Email) == user.Email)
            {
                IdentityResult result = userManager.ChangePasswordAsync(user, data.currentPassword, data.password).Result;
                if (result.Succeeded)
                {
                    signInManager.SignInAsync(user, false);
                    return new OperationResult { success = true, text = "Password updated successfully" };
                }
                else
                {
                    return new OperationResult(result);
                }
            }
            return new OperationResult { success = false, text = "User couldn't be found" };
        }

        [Authorize(Policy = Claims.Users.UserDelete)]
        [HttpDelete]
        public OperationResult delete([FromBody] List<string> userIds)
        {
            List<IdentityUser> users = userIds.Select(id => userService.getUserById(id).Result).ToList();
            if (!users.Contains(null))
            {
                foreach (IdentityUser user in users)
                {
                    IdentityResult result = userManager.DeleteAsync(user).Result;
                    if (!result.Succeeded)
                    {
                        return new OperationResult(result);
                    }
                }
                return new OperationResult { success = true, text = userIds.Count > 1 ? "Users deleted successfully" : "User deleted successfully" };
            }
            return new OperationResult { success = false, text = "One of the users couldn't be found" };
        }
    }
}
