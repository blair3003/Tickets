using System.ComponentModel.DataAnnotations;

namespace Tickets.Data.Models
{
    public class CreateTicketInput
    {
        [Required]
        [StringLength(100, ErrorMessage = "Maximum length is {1}")]
        [Display(Name = "Summary")]
        public string? Summary { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string? Description { get; set; }
    }
}
