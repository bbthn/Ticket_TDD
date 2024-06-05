using Moq;
using NUnit.Framework;
using AutoMapper;
using System;
using System.Threading.Tasks;
using Application.Interfaces.Repository;
using Application.Services;
using Core.Domain.Entities;
using Core.Application.Dtos;
using System.Linq.Expressions;

[TestFixture]
public class TicketServiceTests
{
    private Mock<ITicketRepository> _ticketRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private TicketService _ticketService;

    [SetUp]
    public void SetUp()
    {
        _ticketRepositoryMock = new Mock<ITicketRepository>();
        _mapperMock = new Mock<IMapper>();
        _ticketService = new TicketService(_ticketRepositoryMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task GetTicketAsync_TicketFound_ReturnsTicketDto()
    {
        // Arrange
        var pnr = "ABC123";
        var ticket = new Ticket { Pnr = pnr, TicketNumber = "123456", Status = 1 };
        var ticketDto = new TicketDto { Pnr = pnr, TicketNumber = "123456", Status = 1 };
        _ticketRepositoryMock.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync(ticket);
        _mapperMock.Setup(x => x.Map<TicketDto>(ticket)).Returns(ticketDto);

        // Act
        var result = await _ticketService.GetTicketAsync(pnr);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(ticketDto, result);
    }

    [Test]
    public async Task GetTicketAsync_TicketNotFound_ReturnsNull()
    {
        // Arrange
        var pnr = "ABC123";
        _ticketRepositoryMock.Setup(x => x.GetSingleAsync(It.IsAny<Expression<Func<Ticket, bool>>>())).ReturnsAsync((Ticket)null);

        // Act
        var result = await _ticketService.GetTicketAsync(pnr);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public async Task VoidTicketAsync_TicketVoided_ReturnsVoidedTicketDto()
    {
        // Arrange
        var ticketDto = new TicketDto { Pnr = "ABC123", TicketNumber = "123456", Status = 1 };
        var ticket = new Ticket { Pnr = "ABC123", TicketNumber = "123456", Status = 0 };
        _mapperMock.Setup(x => x.Map<Ticket>(ticketDto)).Returns(ticket);
        _ticketRepositoryMock.Setup(x => x.UpdateAsync(ticket)).ReturnsAsync(ticket);
        _mapperMock.Setup(x => x.Map<TicketDto>(ticket)).Returns(ticketDto);

        // Act
        var result = await _ticketService.VoidTicketAsync(ticketDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(ticketDto, result);
    }

    [Test]
    public async Task VoidTicketAsync_TicketNotVoided_ReturnsNull()
    {
        // Arrange
        var ticketDto = new TicketDto { Pnr = "ABC123", TicketNumber = "123456", Status = 1 };
        _mapperMock.Setup(x => x.Map<Ticket>(ticketDto)).Returns(new Ticket());
        _ticketRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Ticket>())).ReturnsAsync((Ticket)null);

        // Act
        var result = await _ticketService.VoidTicketAsync(ticketDto);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void ValidateTicket_ValidTicketNumber_ReturnsTrue()
    {
        // Arrange
        var pnr = "ABC123";
        var ticketNumber = "ABC123";

        // Act
        var result = _ticketService.ValidateTicket(pnr, ticketNumber);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void ValidateTicket_InvalidTicketNumber_ReturnsFalse()
    {
        // Arrange
        var pnr = "ABC123";
        var ticketNumber = "654321";

        // Act
        var result = _ticketService.ValidateTicket(pnr, ticketNumber);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task ReissueTicket_TicketReissued_ReturnsUpdatedTicketDto()
    {
        // Arrange
        var ticketDto = new TicketDto { Pnr = "ABC123", TicketNumber = "123456", Status = 1 };
        var newFlightId = Guid.NewGuid();
        var updatedTicket = new Ticket { Pnr = "ABC123", TicketNumber = "123456", Status = 1, FlightId = newFlightId };
        _mapperMock.Setup(x => x.Map<Ticket>(ticketDto)).Returns(updatedTicket);
        _ticketRepositoryMock.Setup(x => x.UpdateAsync(updatedTicket)).ReturnsAsync(updatedTicket);
        var expectedUpdatedTicketDto = new TicketDto { Pnr = "ABC123", TicketNumber = "123456", Status = 1, FlightId = newFlightId };
        _mapperMock.Setup(x => x.Map<TicketDto>(updatedTicket)).Returns(expectedUpdatedTicketDto);

        // Act
        var result = await _ticketService.ReissueTicket(ticketDto, newFlightId);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(expectedUpdatedTicketDto, result);
    }

    [Test]
    public async Task ReissueTicket_TicketNotReissued_ReturnsNull()
    {
        // Arrange
        var ticketDto = new TicketDto { Pnr = "ABC123", TicketNumber = "123456", Status = 1 };
        var newFlightId = Guid.NewGuid();
        _mapperMock.Setup(x => x.Map<Ticket>(ticketDto)).Returns(new Ticket());
        _ticketRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Ticket>())).ReturnsAsync((Ticket)null);

        // Act
        var result = await _ticketService.ReissueTicket(ticketDto, newFlightId);

        // Assert
        Assert.IsNull(result);
    }
}
