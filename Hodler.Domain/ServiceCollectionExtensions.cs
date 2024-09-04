using Hodler.Domain.Transactions.Services;
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

        return services;
    }
}