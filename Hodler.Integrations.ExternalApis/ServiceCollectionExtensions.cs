using Hodler.Domain.Portfolio.Ports.BitPandaApi;
using Hodler.Domain.Shared.Ports.DiaDataApi;
using Hodler.Integrations.ExternalApis.Portfolio.SyncWithExchange;
using Hodler.Integrations.ExternalApis.PriceFeed;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integrations.ExternalApis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services)
    {
        
// Tell Mapster to scan this assambly searching for the Mapster.IRegister
// classes and execute them
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);
        services.AddTransient<IDiaDataApiClient, DiaDataApiClient>();
        services.AddTransient<IBitPandaApiClient, BitPandaApiClient>();
        
        return services;
    }
}

