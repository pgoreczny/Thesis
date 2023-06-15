namespace Thesis.Models.Calendar
{
    public class Week
    {
        public List<Day> days = new List<Day>();
    }
    public class Day
    {
        public int dayNumber { get; set; }
        public List<Reminder> reminders = new List<Reminder>();
    }
}
