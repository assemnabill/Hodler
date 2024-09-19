using System.Collections;
using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public class Transactions : ITransactions
{
    private readonly IReadOnlyCollection<Transaction> _transactions;
    public int Count => _transactions.Count;

    public Transactions(IEnumerable<Transaction> transactions)
    {
        _transactions = transactions
            .OrderBy(x => x.Timestamp)
            .ToList();

        EnsureNoDuplicates();
    }

    private void EnsureNoDuplicates()
    {
        // TODO
    }

    public IEnumerator<Transaction> GetEnumerator() => _transactions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public SyncResult<ITransactions> Sync(IEnumerable<Transaction> transactions)
    {
        var changed = false;
        var currentTransactions = _transactions.ToList();

        foreach (var transaction in transactions)
        {
            if (currentTransactions.Any(x => x.Equals(transaction)))
                continue;

            currentTransactions.Add(transaction);
            changed = true;
        }

        return new SyncResult<ITransactions>(changed, new Transactions(currentTransactions));
    }

    public async Task<PortfolioSummary> GetSummaryReportAsync(
        ICurrentPriceProvider currentPriceProvider,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(currentPriceProvider);

        if (_transactions.Count == 0)
        {
            return new PortfolioSummary(0, 0, 0, 0, 0, 0, 0, 0, 0);
        }

        var netInvestedFiat = _transactions.Sum(t => t.FiatAmount.Amount);
        var totalBtcInvestment = _transactions.Sum(t => t.BtcAmount.Amount);

        var currentBtcPrice = await currentPriceProvider.GetCurrentPriceAsync(cancellationToken);
        var currentValue = totalBtcInvestment * currentBtcPrice;
        var totalProfitFiat = currentValue - netInvestedFiat;
        var totalProfitPercentage = (totalProfitFiat / netInvestedFiat) * 100;

        var avgBtcPrice = _transactions.Average(x => x.MarketPrice);

        var taxFreeTransactions = _transactions
            .Where(t => t.Timestamp <= DateTimeOffset.UtcNow.AddYears(-1));

        var taxFreeTotalBtcInvestment = taxFreeTransactions.Sum(t => t.BtcAmount.Amount);
        var taxFreeProfit = taxFreeTotalBtcInvestment * currentBtcPrice;

        return new PortfolioSummary(
            netInvestedFiat,
            totalBtcInvestment,
            currentBtcPrice,
            currentValue,
            totalProfitFiat,
            totalProfitPercentage,
            avgBtcPrice,
            taxFreeProfit,
            taxFreeTotalBtcInvestment
        );
    }
}