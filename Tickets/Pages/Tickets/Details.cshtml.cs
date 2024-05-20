using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tickets.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Tickets.Services;

namespace Tickets.Pages.Tickets
{
    public class DetailsModel : PageModel
    {
        private readonly TicketService _ticketService;
        private readonly IAuthorizationService _authService;

        [BindProperty(SupportsGet = true)]
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public bool CanEditTicket { get; set; } = false;
        public bool CanDeleteTicket { get; set; } = false;

        public DetailsModel(TicketService ticketService, IAuthorizationService authorizationService)
        {
            _ticketService = ticketService;
            _authService = authorizationService;
        }      

        public async Task<IActionResult> OnGetAsync()
        {
            Ticket = await _ticketService.GetTicketByIdAsync(TicketId);

            if (Ticket == null)
            {
                return NotFound();
            }

            var canEditTicketCheck = await _authService.AuthorizeAsync(User, "CanEditTicket");
            var canDeleteTicketCheck = await _authService.AuthorizeAsync(User, "CanDeleteTicket");

            CanEditTicket = canEditTicketCheck.Succeeded;
            CanDeleteTicket = canDeleteTicketCheck.Succeeded;

            return Page();
        }
    }
}
