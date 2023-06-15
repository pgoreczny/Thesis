namespace Thesis.Models.Calendar
{
    public class ReminderInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string text { get; set; }
        public string datestring { get; set; }
        public bool canModify { get; set; }
        public bool error { get; set; }

        public ReminderInfo() { }
        public ReminderInfo(Reminder reminder,bool canModify) { 
            this.id = reminder.id;
            this.name = reminder.name;
            this.text = reminder.text;
            this.datestring = reminder.date.ToString("dd-MM-yyyy");
            this.canModify = canModify;
        }
    }
}
