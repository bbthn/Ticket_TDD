using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Application.Services;
using WebApi.Controllers;
using Application.Interfaces.Repository;
using Core.Application.Interfaces.Services;
using Core.Application.Dtos;
using Core.Application.Models;

[TestFixture]
public class TicketControllerTests
{
    private Mock<ITicketService> _ticketServiceMock;
    private TicketController _ticketController;
    private Mock<IFlightService> _flightServiceMock;

    [SetUp]
    public void SetUp()
    {
        _ticketServiceMock = new Mock<ITicketService>();
        _flightServiceMock = new Mock<IFlightService>();
        _ticketController = new TicketController(_ticketServiceMock.Object,_flightServiceMock.Object);
    }

    [Test]
    public async Task VoidTicketAsync_TicketNotFound_ReturnsNotFound()
    {
        // Arrange
        var pnr = "ABC123";
        var ticketNumber = "TICK123";
        _ticketServiceMock.Setup(x => x.GetTicketAsync(pnr)).ReturnsAsync((TicketDto)null);

        // Act
        var result = await _ticketController.VoidTicketAsync(It.IsAny<VoidTicketRequest>());

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var rest = result as NotFoundObjectResult;
        Assert.AreEqual(rest.Value, "Ticket is not found");
    }

    [Test]
    public async Task VoidTicketAsync_TicketNotMatching_ReturnsBadRequest()
    {
        // Arrange
        var pnr = "ABC123";
        var ticketNumber = "TICK123";
        var ticketDto = new TicketDto { Pnr = pnr, TicketNumber = "DIFF123" };
        _ticketServiceMock.Setup(x => x.GetTicketAsync(pnr)).ReturnsAsync(ticketDto);
        _ticketServiceMock.Setup(x => x.ValidateTicket(ticketDto.TicketNumber, ticketNumber)).Returns(false);

        // Act
        var result = await _ticketController.VoidTicketAsync(It.IsAny<VoidTicketRequest>());

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var rest = result as BadRequestObjectResult;
        Assert.AreEqual(rest.Value, "Ticket and Pnr is not matching");
    }

    [Test]
    public async Task VoidTicketAsync_TicketVoidFailed_ReturnsBadRequest()
    {
        // Arrange
        var pnr = "ABC123";
        var ticketNumber = "TICK123";
        var ticketDto = new TicketDto { Pnr = pnr, TicketNumber = ticketNumber };
        _ticketServiceMock.Setup(x => x.GetTicketAsync(pnr)).ReturnsAsync(ticketDto);
        _ticketServiceMock.Setup(x => x.ValidateTicket(ticketDto.TicketNumber, ticketNumber)).Returns(true);
        _ticketServiceMock.Setup(x => x.VoidTicketAsync(ticketDto)).ReturnsAsync((TicketDto)null);

        // Act
        var result = await _ticketController.VoidTicketAsync(It.IsAny<VoidTicketRequest>());

        
        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var rest = result as BadRequestObjectResult;
        Assert.AreEqual(rest.Value, "Ticket is not voided");
    }

    [Test]
    public async Task VoidTicketAsync_TicketVoidSuccess_ReturnsOk()
    {
        // Arrange
        var pnr = "ABC123";
        var ticketNumber = "TICK123";
        var ticketDto = new TicketDto { Pnr = pnr, TicketNumber = ticketNumber };
        _ticketServiceMock.Setup(x => x.GetTicketAsync(pnr)).ReturnsAsync(ticketDto);
        _ticketServiceMock.Setup(x => x.ValidateTicket(ticketDto.TicketNumber, ticketNumber)).Returns(true);
        _ticketServiceMock.Setup(x => x.VoidTicketAsync(ticketDto)).ReturnsAsync(ticketDto);

        // Act
        var result = await _ticketController.VoidTicketAsync(It.IsAny<VoidTicketRequest>());

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var rest = result as OkObjectResult;
        Assert.AreEqual(rest.Value, "Ticket voided successfully");
    }



    [Test]
    public async Task ReissueTicketAsync_ReissueTicketSuccess_ReturnsOk()
    { 
        // Arrange
        var pnr = "ABC123";
        var ticketNumber = "TICK123";
        var req = new ReissueTicketRequest { Pnr = pnr, TicketNumber = ticketNumber };
        var ticketDto = new TicketDto { Pnr = pnr, TicketNumber = ticketNumber };
        _ticketServiceMock.Setup(x => x.GetTicketAsync(pnr)).ReturnsAsync(ticketDto);
        _ticketServiceMock.Setup(x => x.ValidateTicket(ticketDto.TicketNumber, ticketNumber)).Returns(true);
        _flightServiceMock.Setup(x => x.GetFlightOptions(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(new List<FlightDto>());

        //ACT
        var result = await _ticketController.ReissueTicketAsync(req);

        //assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var rest = result as OkObjectResult;

    }
     [Test]
    public async Task ReissueTicketAsync_TicketNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new ReissueTicketRequest
        {
            Pnr = "ABC123",
            TicketNumber = "TICK123",
            NewDeparture = "IST",
            NewDestination = "LHR",
            DateTime = DateTime.Now
        };
        _ticketServiceMock.Setup(x => x.GetTicketAsync(request.Pnr)).ReturnsAsync((TicketDto)null);

        // Act
        var result = await _ticketController.ReissueTicketAsync(request);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var rest = result as NotFoundObjectResult;
        Assert.AreEqual(rest.Value, "Ticket is not found");

    }

    [Test]
    public async Task ReissueTicketAsync_TicketNotMatching_ReturnsBadRequest()
    {
        // Arrange
        var request = new ReissueTicketRequest
        {
            Pnr = "ABC123",
            TicketNumber = "TICK123",
            NewDeparture = "IST",
            NewDestination = "LHR",
            DateTime = DateTime.Now
        };
        var ticketDto = new TicketDto { Pnr = request.Pnr, TicketNumber = "DIFF123" };
        _ticketServiceMock.Setup(x => x.GetTicketAsync(request.Pnr)).ReturnsAsync(ticketDto);
        _ticketServiceMock.Setup(x => x.ValidateTicket(ticketDto.TicketNumber, request.TicketNumber)).Returns(false);

        // Act
        var result = await _ticketController.ReissueTicketAsync(request);

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var rest = result as BadRequestObjectResult;
        Assert.AreEqual(rest.Value, "Ticket and Pnr is not matching");
    }

    [Test]
    public async Task ReissueTicketAsync_FlightOptionsNotFound_ReturnsNotFound()
    {
        // Arrange
        var request = new ReissueTicketRequest
        {
            Pnr = "ABC123",
            TicketNumber = "TICK123",
            NewDeparture = "IST",
            NewDestination = "LHR",
            DateTime = DateTime.Now
        };
        var fopt = new List<FlightDto>();
        var ticketDto = new TicketDto { Pnr = request.Pnr, TicketNumber = request.TicketNumber };
        _ticketServiceMock.Setup(x => x.GetTicketAsync(request.Pnr)).ReturnsAsync(ticketDto);
        _ticketServiceMock.Setup(x => x.ValidateTicket(ticketDto.TicketNumber, request.TicketNumber)).Returns(true);
        _flightServiceMock.Setup(x => x.GetFlightOptions(request.NewDeparture, request.NewDestination, request.DateTime)).ReturnsAsync(It.IsAny<List<FlightDto>>());

        // Act
        var result = await _ticketController.ReissueTicketAsync(request);

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var rest = result as NotFoundObjectResult;
        Assert.AreEqual(rest.Value, "Flight options are not found");
    }

    [Test]
    public async Task ReissueTicketAsync_FlightOptionsFound_ReturnsOk()
    {
        // Arrange
        var request = new ReissueTicketRequest
        {
            Pnr = "ABC123",
            TicketNumber = "TICK123",
            NewDeparture = "IST",
            NewDestination = "LHR",
            DateTime = DateTime.Now
        };
        var ticketDto = new TicketDto { Pnr = request.Pnr, TicketNumber = request.TicketNumber };
        var fopt = new List<FlightDto>();
        _ticketServiceMock.Setup(x => x.GetTicketAsync(request.Pnr)).ReturnsAsync(ticketDto);
        _ticketServiceMock.Setup(x => x.ValidateTicket(ticketDto.TicketNumber, request.TicketNumber)).Returns(true);
        _flightServiceMock.Setup(x => x.GetFlightOptions(request.NewDeparture, request.NewDestination, request.DateTime)).ReturnsAsync(fopt);

        // Act
        var result = await _ticketController.ReissueTicketAsync(request);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var rest = result as OkObjectResult;
    }

    [Test]
    public async Task ReissueConfirmAsync_TicketReissued_ReturnsOk()
    {
        // Arrange
        var ticketDto = new TicketDto { Pnr = "ABC123", TicketNumber = "123456", Status = 1 };
        var newFlightId = Guid.NewGuid();
        var updatedTicketDto = new TicketDto { Pnr = "ABC123", TicketNumber = "123456", Status = 1, FlightId = newFlightId };
        _ticketServiceMock.Setup(x => x.ReissueTicket(ticketDto, newFlightId)).ReturnsAsync(updatedTicketDto);

        // Act
        var result = await _ticketController.ReissueConfirmAsync(It.IsAny<ReissueConfirmRequest>());

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.AreEqual(updatedTicketDto, okResult.Value);
    }

    [Test]
    public async Task ReissueConfirmAsync_TicketNotFound_ReturnsNotFound()
    {
        // Arrange
        var ticketDto = new TicketDto { Pnr = "ABC123", TicketNumber = "123456", Status = 1 };
        var newFlightId = Guid.NewGuid();
        _ticketServiceMock.Setup(x => x.ReissueTicket(ticketDto, newFlightId)).ReturnsAsync((TicketDto)null);

        // Act
        var result = await _ticketController.ReissueConfirmAsync(It.IsAny<ReissueConfirmRequest>());

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
    }



}
