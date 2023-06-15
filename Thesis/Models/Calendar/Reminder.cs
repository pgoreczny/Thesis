namespace Thesis.Models.Calendar
{
    public class Reminder
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public string color { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public Course? course { get; set; }
        public int? courseId { get; set; }

        public ApplicationUser user { get; set; }
        public string userId { get; set; }
    }
}
