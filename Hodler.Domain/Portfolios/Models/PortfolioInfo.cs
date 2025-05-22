using Hodler.Domain.Portfolios.Models.Transactions;

namespace Hodler.Domain.Portfolios.Models;

public class PortfolioInfo
{
    public PortfolioId Id { get; }
    public IReadOnlyCollection<TransactionInfo> Transactions { get; }

    public PortfolioInfo(PortfolioId id, IReadOnlyCollection<TransactionInfo> transactions)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(transactions);

        Id = id;
        Transactions = transactions;
    }
}