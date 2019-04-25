using BugTracker.Models.Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.AppHelper
{
    public class MailSender
    {
        private ApplicationUserManager UserManager { get; set; }

        private Ticket Model { get; set; }

        public MailSender(ApplicationUserManager userManager, Ticket ticket)
        {
            UserManager = userManager;
            Model = ticket;
        }

        public void Send(string currentUserId)
        {

            string deatilsUrl = $"http://localhost:62930/Ticket/Details/{Model.Id}";

            string subject = $"See Updates @ {Model.Title}";

            string body = $"There are some update to this <b><i>{Model.Title}</i></b> ticket. " +
                $" Please visit the <a href=\"" + deatilsUrl + "\">link</a> to see the details";

            var notifyUsers = Model.NotifyUsers.ToList();

            if (notifyUsers.Count() > 0)
            {
                foreach (var u in notifyUsers)
                {
                    if (u.Id != currentUserId)
                    {
                        UserManager.SendEmail(u.Id, subject, body);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(Model.AssignedToUserId) &&
                Model.AssignedToUserId != currentUserId)
            {
                UserManager.SendEmail(Model.AssignedToUserId, subject, body);
            }
        }
    }
}