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

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private ApplicationDbContext Db;
        private UserManager<ApplicationUser> userManager;


        public TicketController()
        {
            Db = new ApplicationDbContext();
            userManager = new UserManager
                                    <ApplicationUser>
                                         (new UserStore
                                            <ApplicationUser>
                                                    (Db));
        }


        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var tickets = Db
                          .Tickets
                          .Where(p => p.Project.Users.Any(x => x.Id == userId))
                          .ProjectTo<TicketIndexViewModel>().ToList();

            return View(tickets);
        }

        [HttpGet]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var model = new CreateTicketsViewModel();

            model.TicketPriorites = Db.TicketPriorites.Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Id.ToString(),
            }).ToList();

            model.TicketTypes = Db.TicketTypes.Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Id.ToString(),
            }).ToList();

            model.AssignedProject = Db.Projects.Where(p => p.Users.Any(n => n.Id == userId)).Select(n => new SelectListItem
            {
                Text = n.Name,
                Value = n.Id.ToString(),
            }).ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(CreateTicketsViewModel form)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(TicketController.Index));
            }
            var model = new Ticket();
            model.Title = form.Title;
            model.Description = form.Description;
            model.ProjectId = Convert.ToInt32(form.ProjectId);
            model.TicketPriorityId = Convert.ToInt32(form.TicketPriorityId);
            model.TicketTypeId = Convert.ToInt32(form.TicketTypeId);
            model.TicketStatusId = 1;
            model.OwnerUserId = User.Identity.GetUserId();
            Db.Tickets.Add(model);
            Db.SaveChanges();

            return RedirectToAction(nameof(TicketController.Index));
        }

        [HttpGet]
        [Authorize(Roles ="Submitter")]
        public ActionResult PostedByMe()
        {
            var userId = User.Identity.GetUserId();

            var model = Db.Tickets
                .Where(P => P.OwnerUserId == userId)
                .ProjectTo<TicketIndexViewModel>().ToList();

            return View(model);
        }
    }
}