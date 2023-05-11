using Thesis.Areas.Identity.Constants;
using Thesis.Models;
using static Thesis.Areas.Identity.Constants.Claims;

namespace Thesis.Areas.Forum.Models
{
    public class ForumUser
    {
        public string userId { get; set; }
        public bool canEdit { get; set; }
        public bool canEditAny { get; set; }
        public bool canDelete { get; set; }
    }
}
