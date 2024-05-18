using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Tickets.Data;
using Tickets.Data.Models;

namespace Tickets.Pages.Tickets
{
    public class CreateModel : PageModel
    {
        private readonly TicketService _ticketService;

        [BindProperty]
        public CreateTicketInput Input { get; set; } = new CreateTicketInput();

        public CreateModel(TicketService ticketService)
        {
            _ticketService = ticketService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newTicket = await _ticketService.AddTicketAsync(
                new Ticket
                {
                    Summary = Input.Summary,
                    Description = Input.Description,
                    ReporterId = userId
                }
            );

            if (newTicket == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to create ticket.");
                return Page();
            }

            return RedirectToPage("./Details/", new { ticketId = newTicket.TicketId });
        }

    }
}
