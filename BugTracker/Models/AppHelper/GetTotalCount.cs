using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.AppHelper
{
    public class GetTotalCount
    {
        private readonly ApplicationDbContext Db;

        private readonly string CurrentUserId;

        public GetTotalCount(ApplicationDbContext dbcontext)
        {
            Db = dbcontext;
            CurrentUserId = HttpContext.Current.User.Identity.GetUserId();
        }

        public int Project()
        {
            return Db.Projects.ToList().Count();
        }

        public int OpenTicket()
        {
            return Db.Tickets.Where(p => p.TicketStatus.Name == "Open").ToList().Count();
        }
        public int ResolvedTicket()
        {
            return Db.Tickets.Where(p => p.TicketStatus.Name == "Resolved").ToList().Count();
        }
        public int RejectedTicket()
        {
            return Db.Tickets.Where(p => p.TicketStatus.Name == "Rejected").ToList().Count();
        }

        public int AssignedProject()
        {
            return Db.Projects.Where(p => p.Users.Any(u => u.Id == CurrentUserId)).ToList().Count();
        }

        public int CreatedTicket()
        {
            return Db.Tickets.Where(p => p.OwnerUserId == CurrentUserId).ToList().Count();
        }

        public int AssignedTicket()
        {
            return Db.Tickets.Where(p => p.AssignedToUserId == CurrentUserId).ToList().Count();
        }


    }
}