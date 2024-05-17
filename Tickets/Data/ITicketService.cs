using Tickets.Data.Models;

namespace Tickets.Data
{
    public interface ITicketService
    {
        Task<IEnumerable<Ticket>> GetAllTicketsAsync();
        Task<Ticket?> GetTicketsByIdAsync(int id);
        Task<Ticket?> AddTicketAsync(Ticket ticket);
        Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket);
        Task<Ticket?> DeleteTicketAsync(int id);
    }
}
