using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTracker.Models.Domain
{
    public class Comment
    {
        public int Id { get; set; }

        public string CommentDescription { get; set; }

        public DateTime Created { get; set; }

        public int TicketId { get; set; }

        public virtual Ticket Ticket { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Comment()
        {
            Created = DateTime.Now;
        }
    }
}