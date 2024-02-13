using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SeniorPomidor.Core.Services;
using SeniorPomidor.Core.Services.TradingService;
using Tinkoff.InvestApi;

namespace SeniorPomidor.Core;

public static class Registration
{
    public static IServiceCollection AddTradingStuff(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<InvestApiSettings>()
            .Bind(configuration.GetSection(nameof(InvestApiSettings)));
        services.AddInvestApiClient("InvestApiClient", (provider, settings) =>
        {
            var investApiSettings = provider.GetRequiredService<IOptions<InvestApiSettings>>();
            settings.AccessToken = investApiSettings.Value.AccessToken;
        });

        services.AddScoped<ITradingService, TradingService>();
        services.AddKeyedScoped<ITradingService, SemaphoredTradingService>(nameof(SemaphoredTradingService));

        return services;
    }
}