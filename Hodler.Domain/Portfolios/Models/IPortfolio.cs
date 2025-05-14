using Corz.DomainDriven.Abstractions.Models.Bases;
using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Microsoft.Extensions.Internal;

namespace Hodler.Domain.Portfolios.Models;

public interface IPortfolio : IAggregateRoot<IPortfolio>
{
    PortfolioId Id { get; }
    UserId UserId { get; }
    ITransactions Transactions { get; }

    SyncResult<IPortfolio> SyncTransactions(IEnumerable<Transaction> transactions);

    Task<IReadOnlyCollection<ChartSpot>> CalculatePortfolioValueChartAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        FiatCurrency userDisplayCurrency,
        ISystemClock systemClock,
        CancellationToken cancellationToken = default
    );

    Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        FiatCurrency userSettingsCurrency,
        CancellationToken cancellationToken = default
    );

    Task<IResult> AddTransactionAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        TransactionType transactionType,
        DateTime date,
        FiatAmount fiatAmount,
        BitcoinAmount bitcoinAmount,
        CryptoExchangeName? cryptoExchange,
        CancellationToken cancellationToken = default
    );

    IResult RemoveTransaction(TransactionId transactionId);

    Task<IResult> ModifyTransactionAsync(
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        TransactionId transactionId,
        TransactionType newTransactionType,
        DateTime newDate,
        FiatAmount newAmount,
        BitcoinAmount newBitcoinAmount,
        CryptoExchangeName? newCryptoExchange,
        CancellationToken cancellationToken = default
    );
}