using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace BugTracker.Models
{
    public class EmailService : IIdentityMessageService
    {
        public string SmtpHost = "smtp.mailtrap.io";
        public int SmtpPort = 2525;
        public string SmtpUsername = "0340befff66304";
        public string SmtpPassword = "9c8abda3f8d8de";
        public string SmtpFrom = "bugtracker@mail.net";

        public void Send(string to, string body, string subject)
        {
            var message = new MailMessage($"MyBugTracker{SmtpFrom}", to);
            message.Body = body;
            message.Subject = subject;
            message.IsBodyHtml = true;

            var SmtpClient = new SmtpClient(SmtpHost, SmtpPort);
            SmtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);
            SmtpClient.Send(message);

        }

        public Task SendAsync(IdentityMessage message)
        {
            //Caling our sync method in a async way.
            return Task.Run(() =>
            Send(message.Destination, message.Body, message.Subject));
        }
    }
}