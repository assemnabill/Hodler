using Hodler.Domain.CryptoExchange.Services;
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
            .AddKeyedScoped<ICurrentBitcoinPriceProvider, DiaCurrentBitcoinPriceProvider>(BitcoinPriceProvider.Dia)
            .AddKeyedScoped<ICurrentBitcoinPriceProvider, BitPandaCurrentBitcoinPriceProvider>(BitcoinPriceProvider.BitPandaTicker);

        services.AddTransient<ITransactionsQueryService, TransactionsQueryService>();
        services.AddTransient<IBitPandaTransactionParser, BitPandaTransactionParser>();
        services.AddTransient<IKrakenTransactionParser, KrakenTransactionParser>();

        services.AddTransient<IUserSettingsQueryService, UserSettingsQueryService>();
        services.AddTransient<IUserSettingsService, UserSettingsService>();

        return services;
    }
}