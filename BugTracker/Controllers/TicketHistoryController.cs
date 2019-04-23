using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketHistoryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}