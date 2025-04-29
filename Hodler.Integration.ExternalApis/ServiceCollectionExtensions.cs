using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.CryptoExchanges.Ports.CryptoExchangeApis;
using Hodler.Integration.ExternalApis.BitcoinPrices.CurrentBitcoinPrice;
using Hodler.Integration.ExternalApis.BitcoinPrices.HistoricalBitcoinPrice.CoinDesk;
using Hodler.Integration.ExternalApis.Portfolios.SyncWithExchange.BitPanda;
using Hodler.Integration.ExternalApis.Portfolios.SyncWithExchange.Kraken;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Integration.ExternalApis;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExternalApis(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ServiceCollectionExtensions).Assembly);

        services.AddTransient<ICurrentBitcoinPriceProvider, BitPandaCurrentBitcoinPriceProvider>();
        services.AddTransient<IBitPandaTickerApiClient, BitPandaSpotTickerApiClient>();
        services.AddTransient<IBitPandaSpotApiClient, BitPandaSpotApiClient>();
        services.AddTransient<IKrakenApiClient, KrakenApiClient>();
        services.AddTransient<ICoinDeskApiClient, CoinDeskApiClient>();
        services.AddCryptoClients();

        return services;
    }
}