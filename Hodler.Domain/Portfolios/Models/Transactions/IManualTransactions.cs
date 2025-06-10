using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public interface IManualTransactions : IReadOnlyCollection<Transaction>
{
    Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken = default
    );

    SyncResult<IManualTransactions> Sync(List<Transaction> newTransactions);

    IManualTransactions Remove(TransactionId transactionId);

    bool AlreadyExists(Transaction newTransaction);
}