using Application.Interfaces.Services;
using Application.Services;
using Core.Application.Interfaces.Services;
using Core.Application.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            var asmm = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(asmm);
            services.AddValidatorsFromAssembly(asmm);
            services.AddFluentValidationAutoValidation();


            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IFlightService,FlightService>();

            return services;
        }
    }
}
