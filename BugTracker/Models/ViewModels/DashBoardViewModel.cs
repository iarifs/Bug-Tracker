using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.ViewModels
{
    public class DashBoardViewModel
    {
        public int TotalProject { get; set; }
        public int TotalTicketByOpen { get; set; }
        public int TotalTicketByResolved { get; set; }
        public int TotalTicketByRejected { get; set; }

        public int TotalTicketAssigned { get; set; }
        public int TotalProjectAssignedByDev { get; set; }

        public int TotalTicketCreated { get; set; }
        public int TotalProjectAssignedBySub { get; set; }



    }
}