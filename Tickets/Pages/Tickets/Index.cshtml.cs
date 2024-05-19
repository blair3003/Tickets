using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
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
            var isAdmin = User.Claims.Any(c => c.Type == "IsAdmin" && bool.Parse(c.Value) == true);

            if (isAdmin)
            {
                Tickets = await _ticketService.GetAllTicketsAsync();
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Tickets = await _ticketService.GetTicketsByReporterIdAsync(userId!);
            }

            return Page();
        }
    }
}
