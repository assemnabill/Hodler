using Hodler.Application.Portfolio.Services;
using Hodler.Application.Portfolio.Services.SyncWithExchange;
using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.Portfolio.Services;
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