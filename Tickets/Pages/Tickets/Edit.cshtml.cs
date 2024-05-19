using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tickets.Data;
using Tickets.Data.Models;

namespace Tickets.Pages.Tickets
{
    public class EditModel : PageModel
    {
        private readonly TicketService _ticketService;

        [BindProperty(SupportsGet = true)]
        public int TicketId { get; set; }

        [BindProperty]
        public UpdateTicketInput Input { get; set; } = new UpdateTicketInput();

        public EditModel(TicketService ticketService)
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

            Input = new UpdateTicketInput
            {
                Summary = ticket.Summary,
                Description = ticket.Description,
                Category = ticket.Category,
                Status = ticket.Status,
                AssigneeId = ticket.AssigneeId,
                DueDate = ticket.DueDate,
                Priority = ticket.Priority
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var ticket = await _ticketService.GetTicketByIdAsync(TicketId);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Summary = Input.Summary;
            ticket.Description = Input.Description;
            ticket.Category = Input.Category;
            ticket.Status = Input.Status;
            ticket.AssigneeId = Input.AssigneeId;
            ticket.DueDate = Input.DueDate;
            ticket.Priority = Input.Priority;

            var updatedTicket = await _ticketService.UpdateTicketAsync(TicketId, ticket);

            if (updatedTicket == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to update ticket.");
                return Page();
            }

            return RedirectToPage("./Details/", new { ticketId = TicketId });
        }

    }
}
