using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace CBT3_Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IConfigurationRoot configuration = new ConfigurationBuilder()
              .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .Build();

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDataServices(configuration);

        return services;
    }
    
}
