using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickets.Data;
using Tickets.Data.Models;

namespace Tickets.Tests.UnitTests
{
    public class TicketServiceTests
    {
        [Fact]
        public async Task GetAllTicketsAsync_ReturnsAllTickets()
        {
            var mockRepository = new Mock<ITicketRepository>();

            mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(
                    new List<Ticket>
                    {
                        new() { TicketId = 1, Summary = "Ticket 1", ReporterId = "1" },
                        new() { TicketId = 2, Summary = "Ticket 2", ReporterId = "1" },
                        new() { TicketId = 3, Summary = "Ticket 3", ReporterId = "1" },
                    }
                );
            
            var ticketService = new TicketService(mockRepository.Object);

            var result = await ticketService.GetAllTicketsAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(result, t => t.Summary == "Ticket 1");
            Assert.Contains(result, t => t.Summary == "Ticket 2");
            Assert.Contains(result, t => t.Summary == "Ticket 3");

        }

    }
}
