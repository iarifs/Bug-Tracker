using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using BugTracker.Models.Filters;
using Microsoft.AspNet.Identity.Owin;
using BugTracker.Models.AppHelper;

namespace BugTracker.Controllers
{

    [Authorize]
    public class TicketAttachmentController : Controller
    {
        private readonly ApplicationDbContext Db = new ApplicationDbContext();

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
        [CustomActionFilter]
        public ActionResult AddAttachment(int id, TicketDetailsViewModel form)
        {
            var ticket = Db.Tickets.FirstOrDefault(p => p.Id == id);

            var mailHelper = new MailSender(userManager, ticket);

            var currentUserId = User.Identity.GetUserId();

            if (ticket != null && form.MediaUrl != null)
            {
                const string FOLDER = "~/Vault/";
                var userId = User.Identity.GetUserId();
                var fileName = form.MediaUrl.FileName;
                var mappedPath = System.Web.HttpContext.Current.Server.MapPath(FOLDER);
                var filePathWithName = mappedPath + fileName;


                if (!Directory.Exists(FOLDER))
                {
                    Directory.CreateDirectory(mappedPath);
                }

                form.MediaUrl.SaveAs(filePathWithName);

                var model = new Attachment();
                model.Description = form.AttachmentsDetails;
                model.TicketId = id;
                model.UserId = userId;
                model.FilePath = FOLDER + fileName;

                Db.Attachments.Add(model);

                Db.SaveChanges();

                mailHelper.Send(currentUserId);

            }

            return RedirectToAction(nameof(TicketController.Details), nameof(TicketController).ToControllerName(), new { id = id });
        }
    }
}