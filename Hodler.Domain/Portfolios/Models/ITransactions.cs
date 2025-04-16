using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public interface ITransactions : IReadOnlyCollection<Transaction>
{
    Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken
    );

    SyncResult<ITransactions> Sync(IEnumerable<Transaction> transactions);

    Task<FiatAmount> GetPortfolioValueOnDateAsync(
        DateOnly dateOfTransaction,
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        FiatCurrency userDisplayCurrency,
        CancellationToken cancellationToken = default
    );

    ITransactions Add(
        PortfolioId portfolioId,
        TransactionType transactionType,
        DateTimeOffset date,
        FiatAmount fiatAmount,
        BitcoinAmount bitcoinAmount,
        CryptoExchangeName? cryptoExchange,
        out IResult result
    );
}