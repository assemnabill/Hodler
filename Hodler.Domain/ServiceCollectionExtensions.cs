using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.PriceCatalog.Models;
using Hodler.Domain.PriceCatalog.Services;
using Hodler.Domain.User.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services
            .AddKeyedScoped<ICurrentBitcoinPriceProvider, DiaCurrentBitcoinPriceProvider>(
                serviceKey: BitcoinPriceProvider.Dia
            )
            .AddKeyedScoped<ICurrentBitcoinPriceProvider, BitPandaCurrentBitcoinPriceProvider>(
                serviceKey: BitcoinPriceProvider.BitPandaTicker
            );


        services
            .AddTransient<IUserSettingsQueryService, UserSettingsQueryService>()
            .AddTransient<IUserSettingsService, UserSettingsService>();

        return services;
    }
}