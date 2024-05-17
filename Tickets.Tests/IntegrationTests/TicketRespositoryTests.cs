using Microsoft.Extensions.Options;
using Tickets.Data;
using Tickets.Data.Models;

namespace Tickets.Tests.IntegrationTests
{
    public class TicketRespositoryTests(TestEnvironment env) : IClassFixture<TestEnvironment>
    {
        private readonly TestEnvironment _env = env;

        [Fact]
        public async Task GetAllAsync_ReturnsAllTicketsAsync()
        {
            var tickets = new List<Ticket>
            {
                new() { TicketId = 1, Summary = "Ticket 1", ReporterId = _env.TestUser.Id },
                new() { TicketId = 2, Summary = "Ticket 2", ReporterId = _env.TestUser.Id },
                new() { TicketId = 3, Summary = "Ticket 3", ReporterId = _env.TestUser.Id }
            };

            using (var context = _env.CreateContext())
            {
                context.Tickets.AddRange(tickets);
                await context.SaveChangesAsync();
            }

            using (var context = _env.CreateContext())
            {
                var repository = new TicketRepository(context);
                var result = await repository.GetAllAsync();

                Assert.NotNull(result);
                Assert.True(result.Count() >= 3);
                Assert.Contains(result, t => t.Summary == "Ticket 1");
                Assert.Contains(result, t => t.Summary == "Ticket 2");
                Assert.Contains(result, t => t.Summary == "Ticket 3");
            }
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsTicket()
        {
            var ticket = new Ticket
            {
                TicketId = 4,
                Summary = "Test Ticket",
                ReporterId = _env.TestUser.Id
            };

            using (var context = _env.CreateContext())
            {
                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();
            }

            using (var context = _env.CreateContext())
            {
                var repository = new TicketRepository(context);
                var result = await repository.GetByIdAsync(4);

                Assert.NotNull(result);
                Assert.Equal("Test Ticket", result.Summary);
                Assert.Equal(_env.TestUser.Id, result.ReporterId);
            }
        }

        [Fact]
        public async Task AddAsync_CreatesNewTicket()
        {
            var ticket = new Ticket
            {
                TicketId = 5,
                Summary = "New Ticket",
                ReporterId = _env.TestUser.Id
            };

            using (var context = _env.CreateContext())
            {
                var repository = new TicketRepository(context);
                var returnedTicket = await repository.AddAsync(ticket);

                Assert.NotNull(returnedTicket);
            }

            using (var context = _env.CreateContext())
            {
                var result = await context.Tickets.FindAsync(5);

                Assert.NotNull(result);
                Assert.Equal("New Ticket", result.Summary);
                Assert.Equal(_env.TestUser.Id, result.ReporterId);
            }
        }

        [Fact]
        public async Task UpdateAsync_ModifiesExistingTicket()
        {
            var ticket = new Ticket
            {
                TicketId = 6,
                Summary = "Existing Ticket",
                ReporterId = _env.TestUser.Id
            };

            using (var context = _env.CreateContext())
            {
                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();
            }

            using (var context = _env.CreateContext())
            {
                var repository = new TicketRepository(context);

                var ticketToUpdate = await context.Tickets.FindAsync(6);
                Assert.NotNull(ticketToUpdate);

                ticketToUpdate.Summary = "Updated Ticket";
                var orginalUpdatedDate = ticketToUpdate.Updated;

                var result = await repository.UpdateAsync(6, ticketToUpdate);

                Assert.NotNull(result);
                Assert.Equal("Updated Ticket", result.Summary);
                Assert.True(result.Updated > orginalUpdatedDate);
            }
        }

        [Fact]
        public async Task DeleteAsync_RemovesExistingTicket()
        {
            var ticket = new Ticket
            {
                TicketId = 7,
                Summary = "Ticket to Delete",
                ReporterId = _env.TestUser.Id
            };

            using (var context = _env.CreateContext())
            {
                context.Tickets.Add(ticket);
                await context.SaveChangesAsync();
            }

            using (var context = _env.CreateContext())
            {
                var repository = new TicketRepository(context);

                var result = await repository.DeleteAsync(7);
                Assert.NotNull(result);
            }

            using (var context = _env.CreateContext())
            {
                var deletedTicket = await context.Tickets.FindAsync(7);
                Assert.Null(deletedTicket);
            }
        }
    }
}
