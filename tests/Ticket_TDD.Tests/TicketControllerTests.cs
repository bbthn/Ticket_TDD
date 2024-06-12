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
using Microsoft.Extensions.Logging;

[TestFixture]
public class TicketControllerTests
{
    private Mock<ITicketService> _ticketServiceMock;
    private TicketController _ticketController;
    private Mock<IFlightService> _flightServiceMock;
    private Mock<ILogger<TicketController>> _loggerMock;

    [SetUp]
    public void SetUp()
    {
        _ticketServiceMock = new Mock<ITicketService>();
        _flightServiceMock = new Mock<IFlightService>();
        _loggerMock = new Mock<ILogger<TicketController>>();
        _ticketController = new TicketController(_ticketServiceMock.Object,_flightServiceMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task VoidTicketAsync_TicketNotFound_ReturnsNotFound()
    {
        // Arrange
        _ticketServiceMock.Setup(x => x.GetTicketAsync(It.IsAny<string>())).ReturnsAsync((TicketDto)null);

        // Act
        var result = await _ticketController.VoidTicketAsync(new VoidTicketRequest());

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var rest = result as NotFoundObjectResult;
        Assert.AreEqual(rest.Value, "Ticket is not found");
    }

    [Test]
    public async Task VoidTicketAsync_TicketNotMatching_ReturnsBadRequest()
    {
 
        _ticketServiceMock.Setup(x => x.GetTicketAsync(It.IsAny<string>())).ReturnsAsync(new TicketDto());
        _ticketServiceMock.Setup(x => x.ValidateTicket(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        var result = await _ticketController.VoidTicketAsync(new VoidTicketRequest());

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var rest = result as BadRequestObjectResult;
        Assert.AreEqual(rest.Value, "Ticket and Pnr is not matching");
    }

    [Test]
    public async Task VoidTicketAsync_TicketVoidFailed_ReturnsBadRequest()
    {
        // Arrange
        _ticketServiceMock.Setup(x => x.GetTicketAsync(It.IsAny<string>())).ReturnsAsync(new TicketDto());
        _ticketServiceMock.Setup(x => x.ValidateTicket(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _ticketServiceMock.Setup(x => x.VoidTicketAsync(It.IsAny<TicketDto>())).ReturnsAsync((TicketDto)null);

        // Act
        var result = await _ticketController.VoidTicketAsync(new VoidTicketRequest());

        
        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var rest = result as BadRequestObjectResult;
        Assert.AreEqual(rest.Value, "Ticket is not voided");
    }

    [Test]
    public async Task VoidTicketAsync_TicketVoidSuccess_ReturnsOk()
    {
        // Arrange
        _ticketServiceMock.Setup(x => x.GetTicketAsync(It.IsAny<string>())).ReturnsAsync(new TicketDto());
        _ticketServiceMock.Setup(x => x.ValidateTicket(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _ticketServiceMock.Setup(x => x.VoidTicketAsync(It.IsAny<TicketDto>())).ReturnsAsync(new TicketDto());

        // Act
        var result = await _ticketController.VoidTicketAsync(new VoidTicketRequest());

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

        _ticketServiceMock.Setup(x => x.GetTicketAsync(It.IsAny<string>())).ReturnsAsync(new TicketDto());
        _ticketServiceMock.Setup(x => x.ValidateTicket(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        // Act
        var result = await _ticketController.ReissueTicketAsync(new ReissueTicketRequest());

        // Assert
        Assert.IsInstanceOf<BadRequestObjectResult>(result);
        var rest = result as BadRequestObjectResult;
        Assert.AreEqual(rest.Value, "Ticket and Pnr is not matching");
    }

    [Test]
    public async Task ReissueTicketAsync_FlightOptionsNotFound_ReturnsNotFound()
    {
        // Arrange
        _ticketServiceMock.Setup(x => x.GetTicketAsync(It.IsAny<string>())).ReturnsAsync(new TicketDto());
        _ticketServiceMock.Setup(x => x.ValidateTicket(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _flightServiceMock.Setup(x => x.GetFlightOptions(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync((List<FlightDto>)null);

        // Act
        var result = await _ticketController.ReissueTicketAsync(new ReissueTicketRequest());

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var rest = result as NotFoundObjectResult;
        Assert.AreEqual(rest.Value, "Flight options are not found");
    }

    [Test]
    public async Task ReissueTicketAsync_FlightOptionsFound_ReturnsOk()
    {
        // Arrange
        var fopt = new List<FlightDto>();
        _ticketServiceMock.Setup(x => x.GetTicketAsync(It.IsAny<string>())).ReturnsAsync(new TicketDto());
        _ticketServiceMock.Setup(x => x.ValidateTicket(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _flightServiceMock.Setup(x => x.GetFlightOptions(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>())).ReturnsAsync(fopt);

        // Act
        var result = await _ticketController.ReissueTicketAsync(new ReissueTicketRequest());

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var rest = result as OkObjectResult;
        Assert.AreEqual(rest.Value, new ReissueTicketResponse { Flights = fopt });
    }

    [Test]
    public async Task ReissueConfirmAsync_TicketReissued_ReturnsOk()
    {
        // Arrange
        var updatedTicketDto = new TicketDto() { Id=Guid.NewGuid() };
        _ticketServiceMock.Setup(x => x.ReissueTicket(It.IsAny<TicketDto>(), It.IsAny<Guid>())).ReturnsAsync(updatedTicketDto);
        // Act
        var result = await _ticketController.ReissueConfirmAsync(new ReissueConfirmRequest());

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.AreEqual(updatedTicketDto, okResult.Value);
    }

    [Test]
    public async Task ReissueConfirmAsync_TicketNotFound_ReturnsNotFound()
    {
        // Arrange
        _ticketServiceMock.Setup(x => x.ReissueTicket(It.IsAny<TicketDto>(), It.IsAny<Guid>())).ReturnsAsync((TicketDto)null);

        // Act
        var result = await _ticketController.ReissueConfirmAsync(new ReissueConfirmRequest());

        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
        var res = result as NotFoundObjectResult;
        Assert.AreEqual(res.Value, "Ticket is not found");
    }



}
