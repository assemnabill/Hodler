using Hodler.Domain.CryptoExchange.Ports.CryptoExchangeApis;
using Hodler.Domain.PriceCatalog.Ports;
using Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange.BitPanda;
using Hodler.Integration.ExternalApis.Portfolio.SyncWithExchange.Kraken;
using Hodler.Integration.ExternalApis.PriceCatalog.CurrentBitcoinPrice;
using Hodler.Integration.ExternalApis.PriceCatalog.HistoricalBticoinPrice;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.ExternalApis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);

        services
            .AddSingleton<ICurrentBitcoinPriceProvider, BitPandaCurrentBitcoinPriceProvider>()
            .AddSingleton<IHistoricalBitcoinPriceProvider, CoinCapHistoricalBitcoinPriceProvider>();

        services.AddSingleton<IBitPandaTickerApiClient, BitPandaSpotTickerApiClient>();
        services.AddTransient<IBitPandaSpotApiClient, BitPandaSpotApiClient>();
        services.AddTransient<IKrakenApiClient, KrakenApiClient>();
        services.AddCryptoClients();

        return services;
    }
}