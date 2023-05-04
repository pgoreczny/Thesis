namespace Thesis.Models
{
    public class Answer
    {
        public int id { get; set; }
        public ApplicationUser student { get; set; }
        public DateTime entryDate { get; set; }
        public bool isChecked { get; set; }
        public Guid? fileId { get; set; }
        public File? file { get; set; }
        public bool editable { get; set; }
        public Activity activity { get; set; }
        public int activityId { get; set; }
        public List<ReviewComment> comments { get; set; }

        public int version { get; set; }
    }
}
