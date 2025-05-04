using Corz.DomainDriven.Abstractions.DomainEvents;
using Hodler.Domain.BitcoinPrices.Services;
using Hodler.Domain.Shared.Services;
using Hodler.Domain.Users.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;

namespace Hodler.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {

        services
            .AddTransient<IBitcoinPriceSyncService, BitcoinPriceSyncService>()
            .AddTransient<IPriceCatalogService, PriceCatalogService>()
            .AddTransient<IUserSettingsQueryService, UserSettingsQueryService>()
            .AddTransient<IUserSettingsService, UserSettingsService>();

        services
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();

        services
            .AddSingleton<ISystemClock, SystemClock>();


        return services;
    }
}