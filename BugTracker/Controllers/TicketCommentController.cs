using BugTracker.Models;
using BugTracker.Models.AppHelper;
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

            var mailHelper = new MailSender(userManager, ticket);

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

            mailHelper.Send(currentUserId);


            return RedirectToAction(nameof(TicketController.Details), nameof(TicketController).ToControllerName(), new { id = id });
        }
    }
}