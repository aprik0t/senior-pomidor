using SeniorPomidor.Core;

namespace SeniorPomidor;

public static class Registration
{
    public static void ConfigureServices(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHttpContextAccessor();
        
        services.AddTradingStuff(configuration);
        
        configuration.AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .AddJsonFile("appsettings.Local.json");
    }
}