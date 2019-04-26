using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models.AppHelper
{
    public class GetSelectList
    {
        private readonly ApplicationDbContext Db;
        private readonly UserRolesHelper RoleHelper;

        public GetSelectList(ApplicationDbContext dbContext, UserRolesHelper roleHelper)
        {
            Db = dbContext;
            RoleHelper = roleHelper;
        }

        public List<SelectListItem> OfTicketType()
        {
            return Db
                .TicketTypes
                .Select(n =>
                new SelectListItem
                {
                    Text = n.Name,
                    Value = n.Id.ToString(),
                }).ToList();
        }

        public List<SelectListItem> OfTicketPriority()
        {
            return Db
                 .TicketPriorites
                 .Select(n => new SelectListItem
                 {
                     Text = n.Name,
                     Value = n.Id.ToString(),
                 }).ToList();
        }
        public List<SelectListItem> OfTicketStatus()
        {
            return Db
                .TicketStatuses
                .Select(n => new SelectListItem
                {
                    Text = n.Name,
                    Value = n.Id.ToString(),
                }).ToList();
        }

        public List<SelectListItem> OfDeveloper()
        {
            var developers = RoleHelper.UsersInRole("Developer");

            return developers.Select(n => new SelectListItem
            {
                Text = n.ScreenName,
                Value = n.Id.ToString(),
            }).ToList();

        }

        public List<SelectListItem> OfUsersProject(string userId)
        {
            return Db
                 .Projects
                 .Where(p => p.Users.Any(n => n.Id == userId) &&
                 !p.IsArchived)
                 .Select(n => new SelectListItem
                 {
                     Text = n.Name,
                     Value = n.Id.ToString(),
                 }).ToList();
        }

        public List<SelectListItem> OfAllProject()
        {
            return Db
                 .Projects
                 .Where(p => !p.IsArchived)
                 .Select(n => new SelectListItem
                 {
                     Text = n.Name,
                     Value = n.Id.ToString(),
                 }).ToList();
        }
    }
}