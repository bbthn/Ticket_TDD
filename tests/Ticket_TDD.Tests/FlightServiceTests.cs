using Application.Interfaces.Repository;
using AutoMapper;
using Core.Application.Dtos;
using Core.Application.Interfaces.Services;
using Core.Application.Services;
using Core.Domain.Entities;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;

[TestFixture]
public class FlightServiceTests
{
    private Mock<IFlightRepository> _flightRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private FlightService _flightService;

    [SetUp]
    public void SetUp()
    {
        _flightRepositoryMock = new Mock<IFlightRepository>();
        _mapperMock = new Mock<IMapper>();
        _flightService = new FlightService(_flightRepositoryMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task GetFlightOptions_FlightsFound_ReturnsFlightDtoList()
    {
        // Arrange
        var departure = "IST";
        var destination = "LHR";
        var date = DateTime.Now;
        var flights = new List<Flight>
        {
            new Flight { Departure = departure, Destination = destination, FlightDate = date }
        };
        var flightDtos = new List<FlightDto>
        {
            new FlightDto { Departure = departure, Destination = destination, FlightDate = date }
        };
        _flightRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Flight, bool>>>())).ReturnsAsync(flights);
        _mapperMock.Setup(x => x.Map<List<FlightDto>>(flights)).Returns(flightDtos);

        // Act
        var result = await _flightService.GetFlightOptions(departure, destination, date);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(flightDtos, result);
    }

    [Test]
    public async Task GetFlightOptions_NoFlightsFound_ReturnsNull()
    {
        // Arrange
        var departure = "IST";
        var destination = "LHR";
        var date = DateTime.Now;
        _flightRepositoryMock.Setup(x => x.GetAllAsync(It.IsAny<Expression<Func<Flight, bool>>>())).ReturnsAsync((List<Flight>)null);

        // Act
        var result = await _flightService.GetFlightOptions(departure, destination, date);

        // Assert
        Assert.IsNull(result);
    }

}
