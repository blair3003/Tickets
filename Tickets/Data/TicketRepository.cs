using Microsoft.EntityFrameworkCore;
using Tickets.Data.Models;

namespace Tickets.Data
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            var allTickets = await _context.Tickets.ToListAsync();
            return allTickets;
        }

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            return ticket;
        }

        public async Task<Ticket?> AddAsync(Ticket ticket)
        {
            var users = await _context.Users
                .Where(u => u.Id == ticket.ReporterId || u.Id == ticket.AssigneeId)
                .ToListAsync();

            if (users.All(u => u.Id != ticket.ReporterId) ||
                (ticket.AssigneeId != null && users.All(u => u.Id != ticket.AssigneeId)))
            {
                return null;
            }

            ticket.Created = DateTime.UtcNow;
            ticket.Updated = DateTime.UtcNow;

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket?> UpdateAsync(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return null;
            }

            var users = await _context.Users
                .Where(u => u.Id == ticket.ReporterId || u.Id == ticket.AssigneeId)
                .ToListAsync();

            if (users.All(u => u.Id != ticket.ReporterId) ||
                (ticket.AssigneeId != null && users.All(u => u.Id != ticket.AssigneeId)))
            {
                return null;
            }

            var existingTicket = await _context.Tickets.FindAsync(id);

            if (existingTicket == null)
            {
                return null;
            }

            existingTicket.Summary = ticket.Summary;
            existingTicket.Description = ticket.Description;
            existingTicket.Category = ticket.Category;
            existingTicket.Status = ticket.Status;
            existingTicket.AssigneeId = ticket.AssigneeId;
            existingTicket.ReporterId = ticket.ReporterId;
            existingTicket.DueDate = ticket.DueDate;
            existingTicket.Priority = ticket.Priority;

            existingTicket.Updated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingTicket;
        }

        public async Task<Ticket?> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return null;
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }
    }
}
