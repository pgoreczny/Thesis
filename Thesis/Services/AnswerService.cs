using Microsoft.EntityFrameworkCore;
using System.Text;
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

        public string parseAnswerText(string answerText)
        {
            List<string> words = new List<string>();
            string currentWord = "";
            bool openTag = false;
            foreach (char letter in answerText)
            {
                if (letter == '<')
                {
                    openTag = true;
                }
                if (letter == '>')
                {
                    openTag = false;
                }
                currentWord += letter;
                if (letter == ' ' && !openTag)
                {
                    words.Add(currentWord);
                    currentWord = "";
                }
            }
            words.Add(currentWord);
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            foreach (string word in words)
            {
                if (word[0] != '<')
                {
                    sb.Append("<span class=\"word\" id=\"word");
                    sb.Append(counter);
                    counter++;
                    sb.Append("\">");
                    sb.Append(word);
                    sb.Append(' ');
                    sb.Append("</span>");
                }
                else
                {
                    sb.Append(word);
                    sb.Append(' ');
                }
            }
            return sb.ToString();
        }

        public List<Answer> findAnswer(int activity, string userId)
        {
            List<Answer> answers  = context.answers
                .OrderBy(answer => answer.version)
                .Where(answer => answer.student.Id == userId && answer.activityId == activity)
                .ToList();
            return answers;
        }

        public Answer findAnswerWithParents(int id)
        {
            List<Answer> answer = context.answers
                .Where(answer => answer.id == id)
                .Include(answer => answer.comments.OrderBy(comment => comment.wordNumber))                
                .ThenInclude(comment => comment.author)
                .Include(answer => answer.student)
                .Include(answer => answer.activity)
                .ThenInclude(activity => activity.course)
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

        public string saveComment(ReviewComment comment)
        {
            try
            {
                context.reviewComments.Add(comment);
                context.SaveChanges();
            }
            catch(Exception e)
            {
                return e.Message;
            }
            return "";
        }

        public string updateComment(ReviewComment comment)
        {
            try
            {
                context.Entry(context.reviewComments.Find(comment.id)).State = EntityState.Detached;
                context.reviewComments.Update(comment);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "";
        }
        public bool deleteComment(int id)
        {
            if (context.reviewComments.Find(id) != null)
            {
                context.reviewComments.Remove(context.reviewComments.Find(id));
                context.SaveChanges();
                return true;
            }
            else { return false; }
        }

        public ReviewComment? findComment(int id)
        {
            return context.reviewComments.Find(id);
        }
    }
}
