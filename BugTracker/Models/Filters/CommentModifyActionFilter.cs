﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BugTracker.Models.Filters
{
    public class CommentModifyActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var db = new ApplicationDbContext();

            var userId = filterContext.HttpContext.User.Identity.GetUserId();

            int id = Convert.ToInt32(filterContext.ActionParameters["id"]);

            if (id == 0)
            {
                RedirectToUnathorize(filterContext);
            }

            var comment = db.Comments.FirstOrDefault(p => p.Id == id);

            if (comment == null)
            {
                RedirectToUnathorize(filterContext);
            }

            else if (!filterContext.HttpContext.User.IsInRole("Admin") ||
                 !filterContext.HttpContext.User.IsInRole("Project Manager"))
            {

                if (filterContext.HttpContext.User.IsInRole("Developer") &&
                    comment.UserId != userId)
                {
                    RedirectToUnathorize(filterContext);
                }

                else if (filterContext.HttpContext.User.IsInRole("Submitter") &&
                         comment.UserId != userId)
                {
                    RedirectToUnathorize(filterContext);
                }
            }


        }


        private void RedirectToUnathorize(ActionExecutingContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                  { "controller", "Ticket" },
                    { "action", "Index" }
                 });

        }
    }
}