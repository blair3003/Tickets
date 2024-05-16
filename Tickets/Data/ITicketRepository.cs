using Tickets.Data.Models;

namespace Tickets.Data
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket?> AddAsync(Ticket ticket);
        Task<Ticket?> UpdateAsync(int id, Ticket ticket);
        Task<Ticket?> DeleteAsync(int id);
    }
}
