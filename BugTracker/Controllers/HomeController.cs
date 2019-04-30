using BugTracker.Models;
using BugTracker.Models.AppHelper;
using BugTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext Db;
        private GetTotalCount getTotalNum;

        public HomeController()
        {
            Db = new ApplicationDbContext();
            getTotalNum = new GetTotalCount(Db);
        }

        public ActionResult Index()
        {

            var model = new DashBoardViewModel();

            if (User.IsInRole("Admin") || User.IsInRole("Project Manager"))
            {
                model.TotalProject = getTotalNum.Project();
                model.TotalTicketByOpen = getTotalNum.OpenTicket();
                model.TotalTicketByResolved = getTotalNum.ResolvedTicket();
                model.TotalTicketByRejected = getTotalNum.RejectedTicket();
            }
             if (User.IsInRole("Developer"))
            {
                model.TotalProjectAssignedByDev = getTotalNum.AssignedProject();
                model.TotalTicketAssigned = getTotalNum.AssignedTicket();
            }
             if (User.IsInRole("Submitter"))
            {
                model.TotalProjectAssignedBySub = getTotalNum.AssignedProject();
                model.TotalTicketCreated = getTotalNum.CreatedTicket();
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}