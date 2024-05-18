using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tickets.Data;
using Tickets.Data.Models;

namespace Tickets.Pages.Tickets
{
    public class IndexModel : PageModel
    {
        private readonly TicketService _ticketService;

        public List<Ticket> Tickets { get; set; } = [];

        public IndexModel(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Tickets = await _ticketService.GetAllTicketsAsync();

            return Page();
        }
    }
}
