using Hodler.Application.Portfolio.SyncWithExchange.Services;
using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.Portfolio.Services.Sync;
using Hodler.Domain.Shared.Models;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMapster();

        // Tell Mapster to scan this assambly searching for the Mapster.IRegister
        // classes and execute them
        var assembly = typeof(ServiceCollectionExtensions).Assembly;
        TypeAdapterConfig.GlobalSettings.Scan(assembly);

        services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(assembly); });
        services.AddTransient<IPortfolioQueryService, PortfolioQueryService>();

        services.AddKeyedTransient<
            IPortfolioSyncService,
            BitPandaPortfolioSyncService>(CryptoExchange.BitPanda.Name);

        return services;
    }
}