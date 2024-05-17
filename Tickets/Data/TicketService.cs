using Tickets.Data.Models;

namespace Tickets.Data
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
        {
            var tickets = await _repository.GetAllAsync();
            return tickets;
        }

        public Task<Ticket?> GetTicketsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> AddTicketAsync(Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket)
        {
            throw new NotImplementedException();
        }

        public Task<Ticket?> DeleteTicketAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
