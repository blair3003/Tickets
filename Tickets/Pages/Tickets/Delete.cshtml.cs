using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tickets.Data;

namespace Tickets.Pages.Tickets
{
    public class DeleteModel : PageModel
    {
        private readonly TicketService _ticketService;

        [BindProperty(SupportsGet = true)]
        public int TicketId { get; set; }

        public DeleteModel(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var ticket = await _ticketService.GetTicketByIdAsync(TicketId);

            if (ticket == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var deletedTicket = await _ticketService.DeleteTicketAsync(TicketId);

            if (deletedTicket == null)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
