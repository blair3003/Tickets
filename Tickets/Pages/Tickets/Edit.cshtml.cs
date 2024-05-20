using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tickets.Data.Models;
using Tickets.Services;

namespace Tickets.Pages.Tickets
{
    [Authorize("CanEditTicket")]
    public class EditModel : PageModel
    {
        private readonly TicketService _ticketService;
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty(SupportsGet = true)]
        public int TicketId { get; set; }

        [BindProperty]
        public UpdateTicketInput Input { get; set; } = new UpdateTicketInput();

        public List<ApplicationUser> Users { get; set; } = [];

        public EditModel(TicketService ticketService, UserManager<ApplicationUser> userManager)
        {
            _ticketService = ticketService;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var ticket = await _ticketService.GetTicketByIdAsync(TicketId);

            if (ticket == null)
            {
                return NotFound();
            }

            Users = await _userManager.Users.OrderBy(u => u.UserName).ToListAsync();

            Input = new UpdateTicketInput
            {
                Summary = ticket.Summary,
                Description = ticket.Description,
                Category = ticket.Category,
                Status = ticket.Status,
                AssigneeUserName = ticket.Assignee?.UserName,
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

            if (Input.AssigneeUserName != null)
            {
                var assignee = await _userManager.FindByNameAsync(Input.AssigneeUserName);

                if (assignee != null)
                {
                    ticket.AssigneeId = assignee.Id;
                }
                else
                {
                    ModelState.AddModelError(nameof(Input.AssigneeUserName), "Invalid assignee username.");
                    return Page();
                }
            }

            ticket.Summary = Input.Summary;
            ticket.Description = Input.Description;
            ticket.Category = Input.Category;
            ticket.Status = Input.Status;
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