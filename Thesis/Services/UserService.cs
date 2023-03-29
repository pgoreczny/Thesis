using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Thesis.Areas.Identity.Constants;
using Thesis.Areas.Identity.Models;
using Thesis.database;
using Thesis.Models;

namespace Thesis.Services
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly CoursesDBContext context;
        private readonly IHttpContextAccessor accessor;
        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor accessor, CoursesDBContext context)
        {
            this.userManager = userManager;
            this.accessor = accessor;
            this.context = context;
        }

        public async Task<ApplicationUser> getCurrentUser()
        {
            if (accessor?.HttpContext?.User == null)
            {
                return null;
            }
            string username = accessor?.HttpContext?.User.Identity.Name;
            return await userManager.FindByNameAsync(username);
        }

        public async Task<ApplicationUser> getUserById(string userId)
        {
            try
            {
                return await userManager.FindByIdAsync(userId);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ApplicationUser getUserWithCourses(string userId)
        {
            return context.Users
                .Where(user => user.Id == userId)
                .Include(user => user.CourseApplicationUsers)
                .FirstOrDefault();
        }

        public async Task<ApplicationUser> getUserByName(string name)
        {
            return await userManager.FindByNameAsync(name);
        }
        public async Task<IList<string>> getUserRoles()
        {
            return await userManager.GetRolesAsync(await getCurrentUser());
        }

        public async Task<List<String>> getUserClaims()
        {
            var claims = accessor?.HttpContext?.User.Claims
                .Where(claim => claim.Type == CustomClaimTypes.Permission)
                .Select(claim => claim.Value.Replace(CustomClaimTypes.Permission + ": ", ""))
                .ToList();

            return claims;
        }

        public UserModel GetUserModel(ApplicationUser user)
        {
            return new UserModel
            {
                userName = user.UserName,
                email = user.Email,
                id= user.Id,
                emailConfirmed = user.EmailConfirmed,
                roles = userManager.GetRolesAsync(user).Result.ToList()
            };
        }

        public IdentityResult updateUser(ApplicationUser user)
        {
            return userManager.UpdateAsync(user).Result;
        }
    }
}
