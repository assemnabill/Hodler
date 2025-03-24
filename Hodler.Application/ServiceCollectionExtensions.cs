using Hodler.Application.Portfolios.Services;
using Hodler.Application.Portfolios.Services.SyncWithExchange;
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
        services.AddTransient<IPortfolioQueryService, PortfolioQueryService>();

        services
            .AddKeyedTransient<IPortfolioSyncService, BitPandaPortfolioSyncService>(CryptoExchangeNames.BitPanda)
            .AddKeyedTransient<IPortfolioSyncService, KrakenPortfolioSyncService>(CryptoExchangeNames.Kraken);

        return services;
    }
}