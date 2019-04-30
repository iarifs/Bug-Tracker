using BugTracker.Models;
using BugTracker.Models.AppHelper;
using BugTracker.Models.Domain;
using BugTracker.Models.Filters;
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
                return RedirectToAction(nameof(TicketController.Index), nameof(TicketController).ToControllerName());
            }

            var ticket = Db.Tickets.Where(p => !p.Project.IsArchived).FirstOrDefault(p => p.Id == id);

            if(ticket == null)
            {
                return RedirectToAction(nameof(TicketController.Index), nameof(TicketController).ToControllerName());
            }

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

        [HttpPost]
        [CommentModifyActionFilter]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }

            var comment = Db.Comments.FirstOrDefault(p => p.Id == id);

            if (comment == null)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }

            Db.Comments.Remove(comment);
            Db.SaveChanges();

            return RedirectToAction(nameof(TicketController.Details), nameof(TicketController).ToControllerName(), new { id = comment.TicketId });
        }

        [HttpPost]
        [CommentModifyActionFilter]
        public ActionResult Edit(int? id, string description)
        {
            if (!id.HasValue || string.IsNullOrWhiteSpace(description))
            {
                return RedirectToAction(nameof(TicketController.Index),nameof(TicketController).ToControllerName());
            }

            var comment = Db.Comments.FirstOrDefault(p => p.Id == id);
            
            var currentUserId = User.Identity.GetUserId();

            if (comment != null)
            {
                comment.CommentDescription = description;
            }
            Db.SaveChanges();
            
            return RedirectToAction(nameof(TicketController.Details), nameof(TicketController).ToControllerName(), new { id = comment.TicketId });

        }


    }
}