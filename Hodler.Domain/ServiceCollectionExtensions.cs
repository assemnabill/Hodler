using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.User.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<ICurrentPriceProvider, CurrentPriceProvider>();
        
        services.AddTransient<ITransactionsQueryService, TransactionsQueryService>();
        services.AddTransient<IBitPandaTransactionParser, BitPandaTransactionParser>();
        services.AddTransient<IKrakenTransactionParser, KrakenTransactionParser>();

        services.AddTransient<IUserSettingsQueryService, UserSettingsQueryService>();
        services.AddTransient<IUserSettingsService, UserSettingsService>();

        return services;
    }
}