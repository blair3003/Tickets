using System.ComponentModel.DataAnnotations;

namespace Tickets.Data.Models
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        public string? Summary { get; set; }
        public string? Description { get; set; }
        public Category? Category { get; set; }
        public Status? Status { get; set; }

        public string? AssigneeId { get; set; }
        public ApplicationUser? Assignee { get; set; }

        public string? ReporterId { get; set; }
        public ApplicationUser? Reporter { get; set; }

        public DateTime? DueDate { get; set; }
        public Priority? Priority { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
    }

    public enum Category
    {
        Account,
        Network,
        Software,
        Hardware,
        Other
    }

    public enum Status
    {
        New,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "On Hold")]
        OnHold,
        Resolved,
        Closed
    }

    public enum Priority
    {
        Highest,
        High,
        Medium,
        Low,
        Lowest
    }

}
