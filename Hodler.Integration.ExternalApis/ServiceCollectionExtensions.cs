using Hodler.Domain.CryptoExchanges.Ports.CryptoExchangeApis;
using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Integration.ExternalApis.Portfolios.SyncWithExchange.BitPanda;
using Hodler.Integration.ExternalApis.Portfolios.SyncWithExchange.Kraken;
using Hodler.Integration.ExternalApis.PriceCatalogs.CurrentBitcoinPrice;
using Hodler.Integration.ExternalApis.PriceCatalogs.HistoricalBitcoinPrice;
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
        services.AddTransient<ICoinCapApiClient, CoinCapApiClient>();
        services.AddCryptoClients();

        return services;
    }
}