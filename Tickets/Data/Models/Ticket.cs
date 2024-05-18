namespace Tickets.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Status { get; set; }

        public string? AssigneeId { get; set; }
        public virtual ApplicationUser? Assignee { get; set; }

        public string? ReporterId { get; set; }
        public virtual ApplicationUser? Reporter { get; set; }

        public DateTime DueDate { get; set; }
        public string? Priority { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }

}
