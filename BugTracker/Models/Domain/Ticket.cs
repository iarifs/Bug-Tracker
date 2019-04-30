using BugTracker.Models.AppHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }
        
        public int TicketTypeId { get; set; }
        public virtual TicketType TicketType { get; set; }

        public int TicketStatusId { get; set; }
        public virtual TicketStatus TicketStatus { get; set; }

        public int TicketPriorityId { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }

        public string OwnerUserId { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }

        public string AssignedToUserId { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public virtual List<Attachment> Attachments { get; set; }

        public virtual List<History> Histories { get; set; }

        public virtual List<ApplicationUser> NotifyUsers { get; set; }

        public Ticket()
        {
            Comments = new List<Comment>();
            Attachments = new List<Attachment>();
            Histories = new List<History>();
            DateCreated = DateTime.Now;
        }

    }
}