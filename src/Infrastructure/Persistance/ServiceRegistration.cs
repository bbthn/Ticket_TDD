using Application.Interfaces.Repository;
using Core.Domain.Entities.Identity;
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
                opt.UseSqlServer(conf.GetConnectionString("Mssql_local"), mssqlOpt => {
                    mssqlOpt.EnableRetryOnFailure(
                        maxRetryCount: 5);                   
                });
            },
            ServiceLifetime.Scoped);

            services.AddIdentity<AppUser,AppRole>( opt =>
            {
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;

                opt.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<TicketDbContext>();

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IFlightRepository, FlightRepository>();
            return services;
        }
    }
}
