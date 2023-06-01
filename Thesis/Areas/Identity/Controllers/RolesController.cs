using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Thesis.Areas.Identity.Constants;
using Thesis.Models;
using System.Security.Claims;
using Thesis.Areas.Identity.Models;
using Roles = Thesis.Areas.Identity.Constants.Roles;

namespace Thesis.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private List<Breadcrumb> crumbs = new List<Breadcrumb>();
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.signInManager = signInManager;
            crumbs.Add(new Breadcrumb { text = "Home", url = "/" });
        }

        [Authorize(Roles = Roles.Admin)]
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

            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseList));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseAdd));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseEdit));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseDelete));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.ManageUsers));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseList));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseAdd));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseEdit));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.CourseDelete));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.ManageCourses.ManageUsers));

            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.AccessCourse));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.JoinCourse));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.ParticipateInCourse));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.SeeCourses));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.AccessCourse));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.JoinCourse));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.ParticipateInCourse));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.SeeCourses));
            await roleManager.AddClaimAsync(student, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.JoinCourse));
            await roleManager.AddClaimAsync(student, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.ParticipateInCourse));
            await roleManager.AddClaimAsync(student, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.SeeCourses));
            await roleManager.AddClaimAsync(registered, new Claim(CustomClaimTypes.Permission, Claims.UserCourses.SeeCourses));

            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Forum.ReadPost));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Forum.WritePost));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Forum.CommentPost));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Forum.EditYour));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Forum.EditAny));
            await roleManager.AddClaimAsync(admin, new Claim(CustomClaimTypes.Permission, Claims.Forum.Delete));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.Forum.ReadPost));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.Forum.WritePost));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.Forum.CommentPost));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.Forum.EditYour));
            await roleManager.AddClaimAsync(lecturer, new Claim(CustomClaimTypes.Permission, Claims.Forum.EditAny));
            await roleManager.AddClaimAsync(student, new Claim(CustomClaimTypes.Permission, Claims.Forum.ReadPost));
            await roleManager.AddClaimAsync(student, new Claim(CustomClaimTypes.Permission, Claims.Forum.WritePost));
            await roleManager.AddClaimAsync(student, new Claim(CustomClaimTypes.Permission, Claims.Forum.CommentPost));
            await roleManager.AddClaimAsync(student, new Claim(CustomClaimTypes.Permission, Claims.Forum.EditYour));
            await roleManager.AddClaimAsync(registered, new Claim(CustomClaimTypes.Permission, Claims.Forum.ReadPost));
            await roleManager.AddClaimAsync(registered, new Claim(CustomClaimTypes.Permission, Claims.Forum.CommentPost));
        }

        //[Authorize(Policy = Claims.Roles.AssignRole)]
        //public IActionResult AssignRole()
        //{
        //    crumbs.Add(new Breadcrumb { text = "Courses", current = true });
        //    crumbs.Add(new Breadcrumb { text = "Assign role", current = true });
        //    ViewBag.crumbs = crumbs;
        //    List<ApplicationUser> ApplicationUsers = userManager.Users.ToList();
        //    List<UserModel> users = new List<UserModel>(ApplicationUsers.Count);
        //    foreach (ApplicationUser user in ApplicationUsers)
        //    {
        //        users.Add(new UserModel { userName = user.UserName, roles = userManager.GetRolesAsync(user).Result.ToList() });
        //    }
        //    return View("RolesAssignment", users);
        //}

        [HttpPost]
        [Authorize(Policy = Claims.Roles.AssignRole)]
        public async Task<OperationResult> AssignRole([FromBody] Role role)
        {
            ApplicationUser user = await userManager.FindByIdAsync(role.userId);

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
