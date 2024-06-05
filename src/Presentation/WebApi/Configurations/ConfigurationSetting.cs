namespace WebApi
{
    public class ConfigurationSetting
    {

        public static IConfiguration logConfiguration
        {
            get
            {
                return new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile($"Configurations/logsettings.json", optional: false)
                   .AddEnvironmentVariables()
                   .Build();
            }

        }
    }
}
