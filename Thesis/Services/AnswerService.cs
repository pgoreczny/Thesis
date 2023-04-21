using Thesis.database;
using Thesis.Models;

namespace Thesis.Services
{
    public class AnswerService
    {
        private readonly CoursesDBContext context;
        
        public AnswerService(CoursesDBContext context)
        {
            this.context = context;
        }

        public Answer addAnswer(Answer answer)
        {
            context.answers.Add(answer);
            context.SaveChanges();
            return answer;
        }

        public void updateAnswer(Answer answer)
        {
            context.answers.Update(answer);
            context.SaveChanges();
        }

        public Answer findAnswer(int activity, string userId)
        {
            List<Answer> answer  = context.answers
                .Where(answer => answer.student.Id == userId && answer.activityId == activity)
                .ToList();
            if (answer.Count == 0)
            {
                return null;
            }
            else
            {
                return answer[0];
            }
        }

        public bool addFile(Models.File file, int answerId)
        {
            Answer? answer = context.answers.Find(answerId);
            if (answer != null)
            {
                answer.file = file;
                answer.fileId = file.Id;
                context.answers.Update(answer);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
