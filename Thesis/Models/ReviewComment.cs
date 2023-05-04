namespace Thesis.Models
{
    public class ReviewComment
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int AnswerId { get; set; }
        public Answer answer { get; set; }
        public string position { get; set; }
        public int wordNumber { get; set; }
        public ApplicationUser author { get; set; }
    }
}
