using Core.Application.Enums;
using Core.Domain.Entities;
using Infrastructure.Persistance.Context;
using Infrastructure.Persistance.Repository;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Xml;


[TestFixture]
public class GenericRepositoryTests
{
    private DbContextOptions<TicketDbContext> _options;
    private const string connectionString = "Data Source=20.205.129.80,1433;User Id=sa;Password=myPassword55;Initial Catalog=Ticket;Connect Timeout=60;Encrypt=False;Trust Server Certificate=True";


    [SetUp]
    public void Setup()
    {
        _options = new DbContextOptionsBuilder<TicketDbContext>()
            .UseSqlServer(connectionString)
            .Options;
    }

    [Test]
    public async Task GetSingleAsync_Should_Return_Entity_When_Filter_Matches()
    {
        var id = Guid.NewGuid();
        // Arrange
        using (var context = new TicketDbContext(_options))
        {
            var repository = new GenericRepository<Flight>(context);
            var entity = new Flight { Id = id , Departure="İzmir", Destination="Ankara", FlightDate=DateTime.Now.AddDays(18)};
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new TicketDbContext(_options))
        {
            var repository = new GenericRepository<Flight>(context);
            var result = await repository.GetSingleAsync(e => e.Id == id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.Id);
        }
    }
    [Test]
    public async Task GetAllAsync_Should_Return_Entity_When_Filter_Matches()
    {        
        // Arrange
        var departure = "İzmir";

        // Act
        using (var context = new TicketDbContext(_options))
        {
            var repository = new GenericRepository<Flight>(context);
            var result = await repository.GetAllAsync(e => e.Departure==departure);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(departure, result.First().Departure=departure);
        }
    }

    [Test]
    public async Task UpdateAsync_Should_Return_Entity_When_Filter_Matches()
    {
        // Arrange
        var departure = "Samsun";
        var newDeparture = "İzmir";


        // Act
        using (var context = new TicketDbContext(_options))
        {
            var repository = new GenericRepository<Flight>(context);
            List<Flight> results = await repository.GetAllAsync();
            var flight = results.First();
            flight.Departure = newDeparture;
            var updated = await repository.UpdateAsync(flight);
            // Assert
            Assert.IsNotNull(updated);
            Assert.AreEqual(updated.Departure, newDeparture);
        }
    }


    [Test]
    public async Task Add_Entity_My_Test()
    {
        var id = Guid.NewGuid();
        // Arrange
        using (var context = new TicketDbContext(_options))
        {
            var flight = new Flight { Departure="Samsun", Destination="İzmir", FlightDate = DateTime.UtcNow.AddDays(5) };

            var repository = new GenericRepository<Ticket>(context);

            var ticket = new Ticket { TicketNumber = "1111111111", FlightId = Guid.Parse("9AD4C39C-5934-4505-AA63-08DC861CFE16"), Status = (int)TicketStatus.Active, Pnr = "aaaaaaaaa1" };

            await context.AddAsync(ticket);
                await context.SaveChangesAsync();
            
           
        }
    }






}
