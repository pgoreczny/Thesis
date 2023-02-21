using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Thesis.Areas.Identity.Constants;
using Thesis.Areas.Identity.Models;

namespace Thesis.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IHttpContextAccessor accessor;
        public UserService(UserManager<IdentityUser> userManager, IHttpContextAccessor accessor)
        {
            this.userManager = userManager;
            this.accessor = accessor;
        }

        public async Task<IdentityUser> getCurrentUser()
        {
            if (accessor?.HttpContext?.User == null)
            {
                return null;
            }
            string username = accessor?.HttpContext?.User.Identity.Name;
            return await userManager.FindByNameAsync(username);
        }

        public async Task<IdentityUser> getUserById(string userId)
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
        public async Task<IdentityUser> getUserByName(string name)
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

        public UserModel GetUserModel(IdentityUser user)
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

    }
}
