using Application.Interfaces.Repository;
using Infrastructure.Persistance.Context;
using Infrastructure.Persistance.Repository.FlightRepository;
using Infrastructure.Persistance.Repository.TicketRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistance
{
    public static class ServiceRegistration 
    {
        public static IServiceCollection ConfigurePersistance(this IServiceCollection services, IConfiguration conf)
        {
            services.AddDbContext<TicketDbContext>(opt =>
            {
                opt.UseSqlServer(conf.GetConnectionString("Mssql_azure"));
            },
            ServiceLifetime.Scoped);

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IFlightRepository, FlightRepository>();
            return services;
        }
    }
}
