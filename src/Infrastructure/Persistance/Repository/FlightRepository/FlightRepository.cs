using Application.Interfaces.Repository;
using Core.Domain.Entities;
using Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance.Repository.FlightRepository
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(TicketDbContext context) : base(context)
        {
        }
    }
}
