using Hodler.Application.BitcoinPrices.Services;
using Hodler.Application.Portfolios.Services;
using Hodler.Application.Portfolios.Services.BitcoinBlockchain;
using Hodler.Application.Portfolios.Services.SyncWithExchange;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Services;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMapster();


        var assembly = typeof(ServiceCollectionExtensions).Assembly;
        TypeAdapterConfig.GlobalSettings.Scan(assembly);

        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(assembly); });

        services.AddScoped<IPortfolioQueryService, PortfolioQueryService>();
        services.AddScoped<IPortfolioCommandService, PortfolioCommandService>();
        services.AddScoped<IBitcoinBlockchainService, BitcoinBlockchainService>();

        services.AddScoped<IHistoricalBitcoinPriceProvider, HistoricalBitcoinPriceProvider>();

        services
            .AddKeyedScoped<IPortfolioSyncService, BitPandaPortfolioSyncService>(CryptoExchangeName.BitPanda)
            .AddKeyedScoped<IPortfolioSyncService, KrakenPortfolioSyncService>(CryptoExchangeName.Kraken);

        return services;
    }
}