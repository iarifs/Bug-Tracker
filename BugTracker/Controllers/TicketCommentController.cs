using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketCommentController : Controller
    {
        private ApplicationDbContext Db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager userManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        public ActionResult AddComment(int? id, TicketDetailsViewModel form)
        {

            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }

            var ticket = Db.Tickets.FirstOrDefault(p => p.Id == id);

            var currentUserId = User.Identity.GetUserId();

            if (ticket != null)
            {
                var model = new Comment();
                model.CommentDescription = form.CommentDescription;
                model.UserId = User.Identity.GetUserId();
                model.TicketId = ticket.Id;
                ticket.Comments.Add(model);
            }
            Db.SaveChanges();

            //checking that we have developer in our list
            //and if developer edit himself he will not get any notification
            if (!string.IsNullOrWhiteSpace(ticket.AssignedToUserId) &&
                currentUserId != ticket.AssignedToUserId)
            {
                string deatilsUrl = $"http://localhost:62930/Ticket/Details/{ticket.Id}";

                string subject = $"See Updates @ {ticket.Title}";

                string body = $"An user commented  in <b><i>{ticket.Title}</i></b>." +
                    $" Please visit the <a href=\"" + deatilsUrl + "\">link</a> to see the details";

                userManager.SendEmail(ticket.AssignedToUserId, subject, body);
            }

            return RedirectToAction(nameof(TicketController.Details), nameof(TicketController).ToControllerName(), new { id = id });
        }
    }
}