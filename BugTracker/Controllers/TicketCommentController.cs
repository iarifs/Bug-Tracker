using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
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

        [HttpPost]
        public ActionResult AddComment(int? id, TicketDetailsViewModel form)
        {

            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }

            var ticket = Db.Tickets.FirstOrDefault(p => p.Id == id);

            if (ticket != null)
            {
                var model = new Comment();
                model.CommentDescription = form.CommentDescription;
                model.UserId = User.Identity.GetUserId();
                ticket.Comments.Add(model);
            }
            Db.SaveChanges();

            return RedirectToAction(nameof(TicketController.Details), nameof(TicketController).ToControllerName(), new { id = id });
        }
    }
}