
namespace CBT3_Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IConfigurationRoot configuration = new ConfigurationBuilder()
                  .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .Build();
}
