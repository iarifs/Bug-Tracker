using BugTracker.Models.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models.ViewModels
{
    public class TicketDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        [Display(Name = "Created By")]
        public string OwnerName { get; set; }

        [Display(Name = "Ticket Type")]
        public string TicketType { get; set; }

        [Display(Name = "Current Status")]
        public string TicketStatus { get; set; }

        [Display(Name = "Ticket Priority")]
        public string TicketPriority { get; set; }

        [Display(Name =" Developer")]
        public string DeveloperId { get; set; }

        public List<SelectListItem> DeveloperList { get; set; }
        
        [Required]
        [Display(Name ="Comment")]
        public string CommentDescription { get; set; }

        public List<Comment> CommentList { get; set; }

        public string Attachments { get; set; }

        public List<Attachment> AttachmentList { get; set; }

        [Required]
        [Display(Name ="Ticket-Details")]
        public string AttachmentsDetails { get; set; }

        [Required]
        public HttpPostedFileBase MediaUrl { get; set; }
    }

}