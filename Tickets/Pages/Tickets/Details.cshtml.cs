using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tickets.Data.Models;
using Tickets.Data;

namespace Tickets.Pages.Tickets
{
    public class DetailsModel : PageModel
    {
        private readonly TicketService _ticketService;

        [BindProperty(SupportsGet = true)]
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public DetailsModel(TicketService ticketService)
        {
            _ticketService = ticketService;
        }      

        public async Task<IActionResult> OnGetAsync()
        {
            Ticket = await _ticketService.GetTicketByIdAsync(TicketId);

            if (Ticket == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
