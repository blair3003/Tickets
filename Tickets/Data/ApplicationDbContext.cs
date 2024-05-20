using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tickets.Data.Models;

namespace Tickets.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-many relationship for Reporter
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Reporter)
                .WithMany(u => u.ReportedTickets)
                .HasForeignKey(t => t.ReporterId)
                .IsRequired(true)  // only set to false if the reporter can indeed be null
                .OnDelete(DeleteBehavior.Restrict);  // Adjust the delete behavior as necessary

            // Configure one-to-many relationship for Assignee
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Assignee)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssigneeId)
                .IsRequired(false)  // only set to false if the assignee can indeed be null
                .OnDelete(DeleteBehavior.Restrict);  // Adjust the delete behavior as necessary
        }

    }
}
