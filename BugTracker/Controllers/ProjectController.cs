﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Controllers
{
    public class ProjectController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}