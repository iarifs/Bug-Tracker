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

namespace BugTracker.Controllers
{

    [Authorize]
    public class TicketAttachmentController : Controller
    {
        private  readonly ApplicationDbContext Db = new ApplicationDbContext();

       [HttpPost]
        public ActionResult AddAttachment(int id,TicketDetailsViewModel form)
        {
            var ticket = Db.Tickets.FirstOrDefault(p => p.Id == id);
            
            if(ticket != null && form.MediaUrl != null)
            {
                const string FOLDER = "~/Vault/";
                var userId = User.Identity.GetUserId();
                var fileName = form.MediaUrl.FileName;
                var  mappedPath = System.Web.HttpContext.Current.Server.MapPath(FOLDER);
                var filePathWithName = mappedPath + fileName;


                if (!Directory.Exists(FOLDER))
                {
                    Directory.CreateDirectory(mappedPath);
                }

                //if (System.IO.File.Exists(filePathWithName))
                //{
                //    System.IO.File.Delete(filePathWithName);
                //}

                form.MediaUrl.SaveAs(filePathWithName);

                var model = new Attachment();
                model.Description = form.AttachmentsDetails;
                model.TicketId = id;
                model.UserId = userId;
                model.FilePath = FOLDER + fileName;

                Db.Attachments.Add(model);

                Db.SaveChanges();
            }

            return RedirectToAction(nameof(TicketController.Details), nameof(TicketController).ToControllerName(), new { id = id });
        }
    }
}