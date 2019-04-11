using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class TicketIndexViewModel
    {
        public int Id { get; set; }

        public string ProjectName { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string TicketTypeName { get; set; }

        public string TicketStatusName { get; set; }

        public string TicketPriorityName { get; set; }

        public string OwnerUserScreenName { get; set; }

        public string AssignedToUserScreenName { get; set; }
    }
}