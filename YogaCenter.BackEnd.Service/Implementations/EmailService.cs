using MailKit.Security;
using MailKit;

using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Service.Contracts;
using MailKit.Net.Smtp;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class EmailService : IEmailService
    {
        public  void SendEmail(string recipient, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Yoga Center", "yogacenter.contact@gmail.com"));
            message.To.Add(new MailboxAddress("Trainee", recipient));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            message.Body = bodyBuilder.ToMessageBody();

            using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                client.Authenticate("yogacenter.contact@gmail.com", "ltndmfckyoefvhgh");
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
