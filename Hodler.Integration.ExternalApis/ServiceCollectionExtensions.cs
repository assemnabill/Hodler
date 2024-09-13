using Hodler.Domain.Portfolio.Ports.BitPandaApi;
using Hodler.Domain.Shared.Ports.DiaDataApi;
using Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange;
using Hodler.Integration.ExternalApis.PriceFeed;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.ExternalApis;

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

