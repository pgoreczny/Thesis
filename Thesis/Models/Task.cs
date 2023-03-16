namespace Thesis.Models
{
    public class Answer
    {
        public int id { get; set; }
        public ApplicationUser student { get; set; }
        public DateTime entryDate { get; set; }
        public string path { get; set; }
    }
}
