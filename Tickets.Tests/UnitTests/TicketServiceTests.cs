using Moq;
using Tickets.Data.Models;
using Tickets.Data.Repositories;
using Tickets.Services;

namespace Tickets.Tests.UnitTests
{
    public class TicketServiceTests
    {
        [Fact]
        public async Task GetAllTicketsAsync_ReturnsAllTickets()
        {
            var mockRepository = new Mock<ITicketRepository>();
            var tickets = new List<Ticket>
            {
                new() { TicketId = 1, Summary = "Ticket 1", ReporterId = "1" },
                new() { TicketId = 2, Summary = "Ticket 2", ReporterId = "1" },
                new() { TicketId = 3, Summary = "Ticket 3", ReporterId = "1" },
            };

            mockRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(tickets);
            
            var ticketService = new TicketService(mockRepository.Object);

            var result = await ticketService.GetAllTicketsAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, t => t.Summary == "Ticket 1");
            Assert.Contains(result, t => t.Summary == "Ticket 2");
            Assert.Contains(result, t => t.Summary == "Ticket 3");

            mockRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTicketByIdAsync_ReturnsTicket()
        {
            var mockRepository = new Mock<ITicketRepository>();
            var ticketId = 1;
            var expectedTicket = new Ticket { TicketId = ticketId, Summary = "Test Ticket", ReporterId = "1" };

            mockRepository
                .Setup(repo => repo.GetByIdAsync(ticketId))
                .ReturnsAsync(expectedTicket);

            var ticketService = new TicketService(mockRepository.Object);

            var result = await ticketService.GetTicketByIdAsync(ticketId);

            Assert.NotNull(result);
            Assert.Equal(expectedTicket.Summary, result.Summary);
            Assert.Equal(expectedTicket.ReporterId, result.ReporterId);

            mockRepository.Verify(repo => repo.GetByIdAsync(ticketId), Times.Once);
        }
        [Fact]
        public async Task GetTicketsByReporterIdAsync_ReturnsTicketsForReporter()
        {
            var mockRepository = new Mock<ITicketRepository>();
            var tickets = new List<Ticket>
            {
                new() { TicketId = 1, Summary = "Ticket 1", ReporterId = "1" },
                new() { TicketId = 2, Summary = "Ticket 2", ReporterId = "1" },
                new() { TicketId = 3, Summary = "Ticket 3", ReporterId = "2" },
            };

            mockRepository
                .Setup(repo => repo.GetByReporterIdAsync("2"))
                .ReturnsAsync(tickets.Where(t => t.ReporterId == "2").ToList());

            var ticketService = new TicketService(mockRepository.Object);

            var result = await ticketService.GetTicketsByReporterIdAsync("2");

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Contains(result, t => t.Summary == "Ticket 3");

            mockRepository.Verify(repo => repo.GetByReporterIdAsync("2"), Times.Once);
        }

        [Fact]
        public async Task AddTicketAsync_CreatesNewTicket()
        {
            var mockRepository = new Mock<ITicketRepository>();
            var newTicket = new Ticket { TicketId = 4, Summary = "New Ticket", ReporterId = "2" };

            mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Ticket>()))
                .ReturnsAsync((Ticket t) => t);

            var ticketService = new TicketService(mockRepository.Object);

            var result = await ticketService.AddTicketAsync(newTicket);

            Assert.NotNull(result);
            Assert.Equal(newTicket.TicketId, result.TicketId);
            Assert.Equal(newTicket.Summary, result.Summary);
            Assert.Equal(newTicket.ReporterId, result.ReporterId);

            mockRepository.Verify(repo => repo.AddAsync(It.Is<Ticket>(t => t == newTicket)), Times.Once);
        }

        [Fact]
        public async Task UpdateTicketAsync_ModifiesExistingTicket()
        {
            var mockRepository = new Mock<ITicketRepository>();
            var ticketId = 5;
            var updatedTicket = new Ticket { TicketId = ticketId, Summary = "Updated Ticket", ReporterId = "3" };

            mockRepository
                .Setup(repo => repo.UpdateAsync(ticketId, It.IsAny<Ticket>()))
                .ReturnsAsync(updatedTicket);

            var ticketService = new TicketService(mockRepository.Object);

            var result = await ticketService.UpdateTicketAsync(ticketId, updatedTicket);

            Assert.NotNull(result);
            Assert.Equal(updatedTicket.Summary, result.Summary);
            Assert.Equal(updatedTicket.ReporterId, result.ReporterId);

            mockRepository.Verify(repo => repo.UpdateAsync(ticketId, updatedTicket), Times.Once);
        }

        [Fact]
        public async Task DeleteTicketAsync_RemovesExistingTicket()
        {
            var mockRepository = new Mock<ITicketRepository>();
            var ticketId = 7;
            var deletedTicket = new Ticket { TicketId = ticketId, Summary = "Deleted Ticket", ReporterId = "7" };

            mockRepository
                .Setup(repo => repo.DeleteAsync(ticketId))
                .ReturnsAsync(deletedTicket);

            var ticketService = new TicketService(mockRepository.Object);

            var result = await ticketService.DeleteTicketAsync(ticketId);

            Assert.NotNull(result);
            Assert.Equal(deletedTicket.Summary, result.Summary);
            Assert.Equal(deletedTicket.ReporterId, result.ReporterId);

            mockRepository.Verify(repo => repo.DeleteAsync(ticketId), Times.Once);
        }

    }
}
