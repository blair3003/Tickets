using Tickets.Data.Models;

namespace Tickets.Data
{
    public class TicketService
    {
        private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            var tickets = await _repository.GetAllAsync();
            return tickets;
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            return ticket;
        }

        public async Task<Ticket?> AddTicketAsync(Ticket ticket)
        {
            var newTicket = await _repository.AddAsync(ticket);
            return newTicket;
        }

        public async Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket)
        {
            var updatedTicket = await _repository.UpdateAsync(id, ticket);
            return updatedTicket;
        }

        public async Task<Ticket?> DeleteTicketAsync(int id)
        {
            var deletedTicket = await _repository.DeleteAsync(id);
            return deletedTicket;
        }
    }
}
