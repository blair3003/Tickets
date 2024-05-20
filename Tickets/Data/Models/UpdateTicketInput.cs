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

        [EnumDataType(typeof(Category), ErrorMessage = "Invalid Category option.")]
        [Display(Name = "Category")]
        public Category? Category { get; set; }

        [EnumDataType(typeof(Status), ErrorMessage = "Invalid Status option.")]
        [Display(Name = "Status")]
        public Status? Status { get; set; }

        [Display(Name = "Assignee")]
        public string? AssigneeUserName { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        [EnumDataType(typeof(Priority), ErrorMessage = "Invalid Priority option.")]
        [Display(Name = "Priority")]
        public Priority? Priority { get; set; }
    }
}