namespace Thesis.Areas.Identity.Models
{
    public class UserModel
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public bool emailConfirmed { get; set; }
        public List<string> roles = new List<string>();
    }
}
