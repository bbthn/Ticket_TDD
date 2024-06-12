using Serilog;


namespace WebApi.Extensions
{
    public static class ServiceRegistration
    {

        public static IServiceCollection ConfigureSerilog(this IServiceCollection services)
        {
            Serilog.Core.Logger logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(ConfigurationSetting.logConfiguration)
                 .CreateLogger();

            services.AddLogging(x =>
            {
                x.AddSerilog(logger);
            });
            return services;

        }
    }
}
