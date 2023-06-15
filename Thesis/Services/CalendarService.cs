using Microsoft.EntityFrameworkCore;
using System.Linq;
using Thesis.database;
using Thesis.Models;
using Thesis.Models.Calendar;

namespace Thesis.Services
{
    public class CalendarService
    {
        private readonly CoursesDBContext context;
        private readonly UserService userService;
        private readonly CourseService courseService;

        public CalendarService(CoursesDBContext context, UserService userService, CourseService courseService)
        {
            this.context = context;
            this.userService = userService;
            this.courseService = courseService;
        }

        public List<Week> getCalendarModel(DateTime monthYear, int courseId = 0)
        {
            List<Week> weeks = new List<Week>();
            DateTime start = new DateTime(monthYear.Year, monthYear.Month, 1);
            List<Reminder> reminders = courseId > 0 ? getEventsForCalendar(monthYear, courseId) : getEventsForCalendar(monthYear);
            Week week = new Week();
            int padding = 0;
            for (int i = 0; i < (int)(start.DayOfWeek + 6) % 7; i++)
            {
                week.days.Add(new Day { dayNumber = 0 });
                padding++;
            }
            while(monthYear.Month == start.Month)
            {
                if (week.days.Count == 7)
                {
                    weeks.Add(week);
                    week = new Week();
                }
                week.days.Add(new Day { dayNumber = start.Day });
                start = start.AddDays(1);
            }
            weeks.Add(week);

            foreach (Reminder reminder in reminders)
            {
                int weekNumber = (reminder.date.Day + padding - 1) / 7;
                weeks[weekNumber].days[(int)(reminder.date.DayOfWeek + 6) % 7].reminders.Add(reminder);
            }
            return weeks;
        }

        public List<Reminder> getEventsForCalendar(DateTime monthYear, int courseId)
        {
            DateTime start = new DateTime(monthYear.Year, monthYear.Month, 1);
            DateTime end = new DateTime(monthYear.Year, monthYear.Month, DateTime.DaysInMonth(monthYear.Year, monthYear.Month));
            return context.reminders
                .Where(reminder => reminder.courseId == courseId && reminder.date.CompareTo(start) >= 0 && reminder.date.CompareTo(end) <= 0)
                .OrderBy(reminder => reminder.date)
                .ToList();
        }

        public List<Reminder> getEventsForCalendar(DateTime monthYear)
        {
            DateTime start = new DateTime(monthYear.Year, monthYear.Month, 1);
            DateTime end = new DateTime(monthYear.Year, monthYear.Month, DateTime.DaysInMonth(monthYear.Year, monthYear.Month));
            ApplicationUser user = userService.getCurrentUser().Result;
            List<int> userCourses = courseService.getUserCourses(user.Id).Select(course => course.CourseId).ToList();
            userCourses.Add(0);
            return context.reminders
                .Where(reminder => ((reminder.courseId != null && userCourses.Contains((int)reminder.courseId)) || (reminder.userId == user.Id && reminder.courseId == null)) && reminder.date.CompareTo(start) >= 0 && reminder.date.CompareTo(end) <= 0)
                .OrderBy(reminder => reminder.date)
                .ToList();
        }

        public string save(Reminder reminder)
        {
            try
            {
                if (reminder.id == 0)
                {
                    context.reminders.Add(reminder);
                }
                else
                {
                    context.reminders.Update(reminder);
                }
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "";
        }

        public Reminder getReminder(int id)
        {
            List<Reminder> reminders = context.reminders
                .Where(reminder => reminder.id == id)
                .Include(reminder => reminder.course)
                .ToList();
            if (reminders.Count > 0)
            {
                return reminders.First();
            }
            else
            {
                return null;
            }
        }

        public string delete(Reminder reminder)
        {
            try
            {
                context.reminders.Remove(reminder);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }

        public void deleteByCourse(int courseId)
        {
            List<Reminder> reminders = context.reminders.Where(reminder => reminder.courseId == courseId).ToList();
            context.reminders.RemoveRange(reminders);
            context.SaveChanges();
        }
    }
}
