using Thesis.Models;

namespace Thesis.Areas.Forum.Models
{
    public class Post
    {
        public int id { get; set; }
        public string title { get; set; }

        public Thesis.Models.Course? course { get; set; }
        public int? courseId { get; set; }
        public Activity? activity { get; set; }
        public int? activityId { get; set; }
        public DateTime date { get; set; }
        public ApplicationUser author { get; set; }
        public string authorId { get; set; }
        public string content { get; set; }
        public List<PostComment> comments { get; set; }
        public bool edited { get; set; }
        public ApplicationUser? editor { get; set; }
        public string? editorId { get; set; }
        public DateTime editDate { get; set; }
    }
}
