using Thesis.Models;

namespace Thesis.Areas.Forum.Models
{
    public class PostComment
    {
        public int id { get; set; }
        public Post? post { get; set; }
        public int? postId { get; set; }
        public DateTime date { get; set; }
        public ApplicationUser author { get; set; }
        public string authorId { get; set; }
        public string content { get; set; }
        public bool edited { get; set; }
        public ApplicationUser? editor { get; set; }
        public string? editorId { get; set; }
        public DateTime editDate { get; set; }
    }
}
