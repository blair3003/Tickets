using Microsoft.Extensions.Options;
using Tickets.Data;
using Tickets.Data.Models;

namespace Tickets.Tests
{
    public class TicketRespositoryTests(TestEnvironment env) : IClassFixture<TestEnvironment>
    {
        private readonly TestEnvironment _env = env;

        [Fact]
        public async Task GetByIdAsync_ReturnsTicket()
        {
            var ticketId = 1;
            var summary = "Test Ticket";
            var reporterId = "1";

            var createReporter = await _env.CreateUser(reporterId);
            Assert.True(createReporter.Succeeded);

            using (var context = _env.CreateContext())
            {
                context.Tickets.Add(new Ticket { TicketId = ticketId, Summary = summary, ReporterId = reporterId });
                await context.SaveChangesAsync();
            }

            using (var context = _env.CreateContext())
            {
                var repository = new TicketRepository(context);
                var result = await repository.GetByIdAsync(ticketId);

                Assert.NotNull(result);
                Assert.Equal(summary, result.Summary);
                Assert.Equal(reporterId, result.ReporterId);
            }
        }
    }
}
