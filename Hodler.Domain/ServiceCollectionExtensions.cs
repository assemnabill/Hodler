using Hodler.Domain.PriceCatalog.Services;
using Hodler.Domain.User.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services
            .AddSingleton<ICurrentBitcoinPriceProvider, BitPandaCurrentBitcoinPriceProvider>();
        
        services
            .AddTransient<IUserSettingsQueryService, UserSettingsQueryService>()
            .AddTransient<IUserSettingsService, UserSettingsService>();

        return services;
    }
}