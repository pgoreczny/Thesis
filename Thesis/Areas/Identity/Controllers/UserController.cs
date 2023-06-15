using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserService userService;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public UserController(UserManager<ApplicationUser> userManager,
            UserService userService,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.userService = userService;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
            this.emailSender = emailSender;
        }

        [Authorize(Policy = Claims.Users.UserList)]
        public IActionResult UserList()
        {
            crumbs.Add(new Breadcrumb { text = "users", current = true });
            ViewBag.crumbs = crumbs;
            List<ApplicationUser> ApplicationUsers = userManager.Users.ToList();
            List<UserModel> users = new List<UserModel>(ApplicationUsers.Count);
            foreach (ApplicationUser user in ApplicationUsers)
            {
                users.Add(userService.GetUserModel(user));
            }
            return View("UserList", users);
        }
        [Authorize(Policy = Claims.Users.UserEdit)]
        public IActionResult editUser([FromQuery(Name = "id")] string id)
        {
            crumbs.Add(new Breadcrumb { text = "Users", url = "/Identity/User/UserList" });
            crumbs.Add(new Breadcrumb { text = "Edit user", current = true });
            ViewBag.crumbs = crumbs;
            ApplicationUser ApplicationUser = userManager.Users.Where(user => user.Id == id).FirstOrDefault();
            UserModel user = userService.GetUserModel(ApplicationUser);
            IList<string> userroles = userManager.GetRolesAsync(ApplicationUser).Result;
            List<Role> roles = roleManager.Roles.Select(role => new Role { name = role.Name, isOn = userroles.Contains(role.Name) }).ToList();
            return View("UserEdit", (user, roles));
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public IActionResult myProfile()
        {

            ApplicationUser ApplicationUser = userService.getCurrentUser().Result;
            UserModel user = userService.GetUserModel(ApplicationUser);
            IList<string> userroles = userManager.GetRolesAsync(ApplicationUser).Result;
            List<Role> roles = roleManager.Roles.Select(role => new Role { name = role.Name, isOn = userroles.Contains(role.Name) }).ToList();
            return View("userProfile", (user, roles));
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public IActionResult getUserInfo([FromQuery(Name = "id")] string id)
        {
            crumbs.Add(new Breadcrumb { text = "User profile", current = true});
            ApplicationUser ApplicationUser = userManager.Users.Where(user => user.Id == id).FirstOrDefault();
            UserModel user = userService.GetUserModel(ApplicationUser);
            return View("UserInfo", user);
        }

        [Authorize(Policy = Claims.Users.UserEdit)]
        [HttpPost]
        public async Task<IActionResult> save([FromForm] UserModel userModel)
        {
            ApplicationUser user = userService.getUserById(userModel.id).Result;
            if (user != null)
            {
                user.Email = userModel.email;
                user.UserName = userModel.userName;
                IdentityResult result = userManager.UpdateAsync(user).Result;
            }
            return LocalRedirect("/Identity/User/editUser?id=" + userModel.id);
        }


        [Authorize(Policy = Claims.Basic.IsRegistered)]
        public async Task<IActionResult> saveProfile([FromForm] UserModel userModel)
        {
            ApplicationUser user = userService.getUserById(userModel.id).Result;
            if (user != null)
            {
                user.Email = userModel.email;
                user.UserName = userModel.userName;
                IdentityResult result = userManager.UpdateAsync(user).Result;
            }
            return LocalRedirect("/Identity/User/myProfile");
        }

        [Authorize(Policy = Claims.Basic.IsRegistered)]
        [HttpPost]
        public OperationResult changePassword([FromBody]PasswordModel data)
        {
            ApplicationUser user = userService.getUserById(data.userId).Result;
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
            List<ApplicationUser> users = userIds.Select(id => userService.getUserById(id).Result).ToList();
            if (!users.Contains(null))
            {
                foreach (ApplicationUser user in users)
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

        [Authorize(Policy = Claims.Users.ConfigureEmails)]
        public ActionResult configure()
        {
            crumbs.Add(new Breadcrumb { text = "Configure e-mails", current = true });
            return View("configureEmails");
        }

        [HttpPost]
        [Authorize(Policy = Claims.Users.ConfigureEmails)]
        public OperationResult saveConfiguration([FromForm] MailCredentials credentials)
        {
            ((EmailSender)emailSender).setCredentials(credentials);
            emailSender.SendEmailAsync(credentials.email, "Test", "This is test of the e-mail configuration");
            return new OperationResult() { success = true, text = "Configuration saved. Wait for the test e-mail" };
        }
    }
}
