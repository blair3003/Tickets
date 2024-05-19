using Microsoft.AspNetCore.Authorization;
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
        private readonly IAuthorizationService _authService;

        public List<Ticket> Tickets { get; set; } = [];

        public IndexModel(TicketService ticketService, IAuthorizationService authService)
        {
            _ticketService = ticketService;
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var canViewAllTicketsCheck = await _authService.AuthorizeAsync(User, "CanViewAllTickets");
            var canViewAllTickets = canViewAllTicketsCheck.Succeeded;

            if (canViewAllTickets)
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
