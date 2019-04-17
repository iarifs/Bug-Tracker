using AutoMapper.QueryableExtensions;
using BugTracker.Models;
using BugTracker.Models.Domain;
using BugTracker.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BugTracker.Models.AppHelper;
using BugTracker.Models.Filters;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        public static string ControllerName = "Ticket";
        private ApplicationDbContext Db;
        private UserManager<ApplicationUser> userManager;
        private UserRolesHelper roleHelper;
        private GetSelectList getSelectList;


        public TicketController()
        {
            Db = new ApplicationDbContext();
            userManager = new UserManager
                                    <ApplicationUser>
                                         (new UserStore
                                            <ApplicationUser>
                                                    (Db));


            roleHelper = new UserRolesHelper(Db);

            getSelectList = new GetSelectList(Db, roleHelper);
        }


        public ActionResult Index()
        {
            //Get the userId of user 
            var userId = User.Identity.GetUserId();

            var userRole = userManager.GetRoles(userId);


            if (User.IsInRole("Admin") ||
                User.IsInRole("Project Manager"))
            {
                var model = Db.Tickets.ProjectTo<TicketIndexViewModel>().ToList();
                model.ForEach(p => p.CanIEdit = true);
                return View(model);
            }

            else if (User.IsInRole("Developer"))
            {
                var developerName = User.Identity.GetScreenName();

                var model = Db
                            .Tickets
                            .Where(p => p.Project.Users.Any(x => x.Id == userId))
                            .ProjectTo<TicketIndexViewModel>().ToList();

                //add canIEdit property for editing their tickets only
                foreach (var x in model)
                {
                    if (x.AssignedToUserScreenName == developerName)
                    {
                        x.CanIEdit = true;
                    }
                }

                return View(model);
            }

            else if (User.IsInRole("Submitter"))
            {
                var submitterName = User.Identity.GetScreenName();
                var model = Db
                          .Tickets
                          .Where(p => p.Project.Users.Any(x => x.Id == userId))
                          .ProjectTo<TicketIndexViewModel>().ToList();

                foreach (var x in model)
                {
                    if (x.OwnerUserScreenName == submitterName)
                    {
                        x.CanIEdit = true;
                    }
                }
                return View(model);
            }

            return View();
        }

        //create ticket 
        [HttpGet]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();

            var model = new CreateTicketsViewModel();

            model.TicketPriorites = getSelectList.OfTicketPriority();

            model.TicketTypes = getSelectList.OfTicketType();

            model.AssignedProject = getSelectList.OfUsersProject(userId);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(CreateTicketsViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var model = new Ticket();
            model.Title = form.Title;
            model.Description = form.Description;
            model.ProjectId = Convert.ToInt32(form.ProjectId);
            model.TicketPriorityId = Convert.ToInt32(form.TicketPriorityId);
            model.TicketTypeId = Convert.ToInt32(form.TicketTypeId);
            //set ticket to open
            model.TicketStatusId = 1;
            model.OwnerUserId = User.Identity.GetUserId();
            Db.Tickets.Add(model);
            Db.SaveChanges();

            return RedirectToAction(nameof(TicketController.Index));
        }

        [HttpGet]
        [Authorize(Roles = "Submitter")]
        public ActionResult PostedByMe()
        {
            var userId = User.Identity.GetUserId();

            var model = Db.Tickets
                .Where(P => P.OwnerUserId == userId)
                .ProjectTo<TicketIndexViewModel>().ToList();

            return View(model);
        }

        [HttpGet]
        [CustomActionFilter]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }
            var userId = User.Identity.GetUserId();

            var ticket = Db.Tickets.FirstOrDefault(p => p.Id == id);

            var model = new CreateTicketsViewModel();

            model.Title = ticket.Title;
            model.Description = ticket.Description;
            model.TicketPriorites = getSelectList.OfTicketPriority();
            model.TicketTypes = getSelectList.OfTicketType();
            model.TicketStatuses = getSelectList.OfTicketStatus();


            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(int? id, CreateTicketsViewModel form)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var model = Db.Tickets.FirstOrDefault(p => p.Id == id);

            model.Title = form.Title;
            model.Description = form.Description;
            model.TicketPriorityId = Convert.ToInt32(form.TicketPriorityId);
            model.TicketTypeId = Convert.ToInt32(form.TicketTypeId);
            model.TicketStatusId = Convert.ToInt32(form.TicketStatusId);
            model.DateUpdated = DateTime.Now;
            Db.SaveChanges();

            return RedirectToAction(nameof(TicketController.Index));
        }

        [HttpGet]
        [CustomActionFilter]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }

            var model = new TicketDetailsViewModel();

            var ticket = Db
                .Tickets
                .Include(p => p.TicketStatus)
                .Include(p => p.TicketType)
                .Include(p => p.TicketPriority)
                .FirstOrDefault(p => p.Id == id);

            if (ticket == null)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }


            if (ticket.AssignedToUserId != null)
            {
                model.DeveloperId = ticket.AssignedToUser.ScreenName;
            }


            model.Id = ticket.Id;
            model.Title = ticket.Title;
            model.DateCreated = ticket.DateCreated;
            model.DateUpdated = ticket.DateUpdated;
            model.Description = ticket.Description;
            model.OwnerName = ticket.OwnerUser.ScreenName;
            model.TicketPriority = ticket.TicketPriority.Name;
            model.TicketStatus = ticket.TicketStatus.Name;
            model.TicketType = ticket.TicketType.Name;
            model.DeveloperList = getSelectList.OfDeveloper();
            model.CommentList = ticket.Comments.Where(p => p.TicketId == ticket.Id).ToList();
            model.AttachmentList = ticket.Attachments.Where(p => p.TicketId == ticket.Id).ToList();

            return View(model);
        }


        [HttpPost]
        public ActionResult Details(int? id, TicketDetailsViewModel form)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }

            var ticket = Db.Tickets.FirstOrDefault(p => p.Id == id);
            var user = Db.Users.FirstOrDefault(p => p.Id == form.DeveloperId);

            if (ticket == null || user == null)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }


            ticket.AssignedToUserId = user.Id;
            Db.SaveChanges();

            return RedirectToAction(nameof(TicketController.Details));

        }

        [HttpGet]
        [Authorize(Roles = "Developer")]
        public ActionResult AssignedToMe()
        {
            var userId = User.Identity.GetUserId();

            var model = Db.Tickets
                .Where(P => P.AssignedToUserId == userId)
                .ProjectTo<TicketIndexViewModel>().ToList();

            return View(model);
        }


        public ActionResult UnAthorizedAccess()
        {
            return View();
        }
    }
}
