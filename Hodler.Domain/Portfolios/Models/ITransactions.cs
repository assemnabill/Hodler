using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public interface ITransactions : IReadOnlyCollection<Transaction>
{
    Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken = default
    );

    SyncResult<ITransactions> Sync(List<Transaction> newTransactions);

    ITransactions Remove(TransactionId transactionId);

    bool AlreadyExists(Transaction newTransaction);
}