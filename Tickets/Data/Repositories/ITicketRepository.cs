using Tickets.Data.Models;

namespace Tickets.Data.Repositories
{
    public interface ITicketRepository
    {
        Task<List<Ticket>> GetAllAsync();
        Task<Ticket?> GetByIdAsync(int id);
        Task<List<Ticket>> GetByReporterIdAsync(string reporterId);
        Task<Ticket?> AddAsync(Ticket ticket);
        Task<Ticket?> UpdateAsync(int id, Ticket ticket);
        Task<Ticket?> DeleteAsync(int id);
    }
}
