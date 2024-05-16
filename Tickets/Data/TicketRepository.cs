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

        public async Task<Ticket?> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            return ticket;
        }

        public Task<Ticket?> AddAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> UpdateAsync(int id, Ticket ticket)
        {
            throw new NotImplementedException();
        }
    }
}
