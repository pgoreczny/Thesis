using MailKit.Net.Smtp;
using MimeKit;
using Thesis.database;
using Thesis.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Thesis.Services
{
    public class EmailSender: IEmailSender
    {
        private readonly CoursesDBContext context;
        public EmailSender(CoursesDBContext context)
        {
            this.context = context;
        }
        public void setCredentials(MailCredentials mailCredentials)
        {
            mailCredentials.id = 1;
            mailCredentials.set = true;
            context.mailCredentials.Update(mailCredentials);
            context.SaveChanges();
        }

        public MailCredentials getCredentials()
        {
            return context.mailCredentials.Find(1);
        }
        public Task SendEmailAsync(string to, string message, string htmlMessage)
        {
            MailCredentials mailCredentials = getCredentials();
            if (mailCredentials == null || !mailCredentials.set)
            {
                throw new ArgumentException();
            }
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(mailCredentials.email, mailCredentials.email));
            email.To.Add(new MailboxAddress(to, to));

            email.Subject = message;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect(mailCredentials.server, 587, false);

                // Note: only needed if the SMTP server requires authentication
                smtp.Authenticate("gorecznypiotr", mailCredentials.password);

                smtp.Send(email);
                smtp.Disconnect(true);
            }
            return Task.CompletedTask;
        }
    }
}
