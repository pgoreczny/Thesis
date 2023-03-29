using Microsoft.AspNetCore.Identity;

namespace Thesis.Models
{
    public class ApplicationUser: IdentityUser
    {
        public List<CourseApplicationUser> CourseApplicationUsers = new List<CourseApplicationUser>();
    }
}
