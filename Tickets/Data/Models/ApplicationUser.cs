using Microsoft.AspNetCore.Identity;

namespace Tickets.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add any additional properties if needed
        public virtual ICollection<Ticket> ReportedTickets { get; set; } = new List<Ticket>();
        public virtual ICollection<Ticket> AssignedTickets { get; set; } = new List<Ticket>();
    }
}
