namespace Thesis.Models
{
    public enum enActivityType
    {
        activity = 0,
        task = 1,
    }
    public class Activity
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime showDate { get; set; }
        public ApplicationUser createdBy { get; set; }
        public ApplicationUser updatedBy { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public enActivityType activityType { get; set; }
        public string text { get; set; }
        public List<File> files = new List<File>();
        public int courseId { get; set; }
        public Course course { get; set; }
        public DateTime dueDate { get; set; }
        public bool allowText { get; set; }
        public bool allowFile { get; set; }
        public List<Answer> answers { get; set; }

        public Activity()
        {

        }
        public Activity(enActivityType activityType)
        {
            this.activityType = activityType;
        }
    }
}
