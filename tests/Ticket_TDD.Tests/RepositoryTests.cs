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
    private const string connectionString = "Data Source=172.167.76.241,1433;User Id=sa;Password=mypassword55!!;Initial Catalog=PostTicketing;Connect Timeout=60;Encrypt=True;Trust Server Certificate=True";


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
    public async Task testa()
    {
        var id = Guid.NewGuid();
        // Arrange
        using (var context = new TicketDbContext(_options))
        {
            var repository = new GenericRepository<Ticket>(context);
            var entity = new Ticket { TicketNumber="0987654321", FlightId = Guid.Parse("E0EABF5B-879C-4861-616A-08DC84ABB7D8"), Status=1,Pnr="RRRAAADDD1" };


            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }
    }

    [Test]
    public async Task GetAllAsync_Should_Return_Entity_When_Filter_Matches()
    {        
        // Arrange
        var departure = "Samsun";
 

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
            var result = await repository.GetSingleAsync(e => e.Departure == departure);
            result.Departure = newDeparture;
            var updated = await repository.UpdateAsync(result);

            // Assert
            Assert.IsNotNull(updated);
            Assert.AreEqual(updated.Departure, newDeparture);
        }
    }





}
