namespace Thesis.Models
{
    public class Course
    {
        public Course()
        {
            this.CourseApplicationUsers = new List<CourseApplicationUser>();
        }
        public int id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public DateTime createDate { get; set; }
        public DateTime updateDate { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal price { get; set; }
        public ApplicationUser createdBy { get; set; }
        public ApplicationUser updatedBy { get; set; }
        public ICollection<ApplicationUser> users { get; set; }
        public List<Activity> activities { get; set; }
        public List<CourseApplicationUser> CourseApplicationUsers = new List<CourseApplicationUser>();
    }

    public class CourseApplicationUser
    {
        public int CourseId { get; set; }
        public Course course { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }
        public enCourseUserStatus status { get; set; }
    }

    public enum enCourseUserStatus
    {
        waitingForApproval = 0,
        approved = 1,
        finished = 2,
        denied = 3
    }
}
