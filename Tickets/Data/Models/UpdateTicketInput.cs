using System;
using System.ComponentModel.DataAnnotations;

namespace Tickets.Data.Models
{
    public class UpdateTicketInput
    {
        [Required]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Summary")]
        public string? Summary { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Category")]
        public string? Category { get; set; }

        [StringLength(50, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Status")]
        public string? Status { get; set; }

        [Display(Name = "Assignee ID")]
        public string? AssigneeId { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [StringLength(20, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Priority")]
        public string? Priority { get; set; }
    }
}
