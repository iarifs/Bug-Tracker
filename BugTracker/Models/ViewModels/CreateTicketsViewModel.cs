using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Models.ViewModels
{
    public class CreateTicketsViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int TicketTypeId { get; set; }

        [Required]
        [Display(Name = "Priority")]
        public int TicketPriorityId { get; set; }

        public List<SelectListItem> AssignedProject { get; set; }

        public List<SelectListItem> TicketTypes { get; set; }
        
        public List<SelectListItem> TicketPriorites { get; set; }
    }
}