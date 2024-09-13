using System.Collections;
using Hodler.Domain.Portfolio.Services;

namespace Hodler.Domain.Portfolio.Models;

public class Transactions : ITransactions
{
    private readonly List<Transaction> _transactions;
    public int Count => _transactions.Count;

    public Transactions(IEnumerable<Transaction> transactions)
    {
        _transactions = transactions.ToList();
    }

    public IEnumerator<Transaction> GetEnumerator() => _transactions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public async Task<TransactionsSummaryReport> GetSummaryReportAsync(
        ICurrentPriceProvider currentPriceProvider,
        CancellationToken cancellationToken)
    {
        var netInvestedFiat = _transactions.Sum(t => t.FiatAmount);
        var totalBtcInvestment = _transactions.Sum(t => t.BtcAmount);

        var currentBtcPrice = await currentPriceProvider.GetCurrentPriceAsync(cancellationToken);
        var currentValue = totalBtcInvestment * currentBtcPrice;
        var totalProfitFiat = currentValue - netInvestedFiat;
        var totalProfitPercentage = (totalProfitFiat / netInvestedFiat) * 100;


        var avgBtcPrice = _transactions.Average(x => x.MarketPrice);


        var taxFreeTransactions = _transactions
            .Where(t => t.Timestamp <= DateTimeOffset.UtcNow.AddYears(-1));

        var taxFreeTotalBtcInvestment = taxFreeTransactions.Sum(t => t.BtcAmount);
        var taxFreeProfit = taxFreeTotalBtcInvestment * currentBtcPrice;

        return new TransactionsSummaryReport(
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

    public ITransactions Sync(IEnumerable<Transaction> transactions)
    {
        _transactions.AddRange(transactions);
        return this;
    }
}