using Hodler.Domain.CryptoExchange.Ports.CryptoExchangeApis;
using Hodler.Domain.Shared.Ports.DiaDataApi;
using Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange.BitPanda;
using Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange.Kraken;
using Hodler.Integration.ExternalApis.PriceCatalog;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.ExternalApis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);

        services.AddTransient<IDiaDataApiClient, DiaDataApiClient>();
        services.AddTransient<IBitPandaApiClient, BitPandaApiClient>();
        services.AddTransient<IKrakenApiClient, KrakenApiClient>();
        services.AddCryptoClients();

        return services;
    }
}