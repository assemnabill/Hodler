using System.Collections;

namespace Hodler.Domain.Transactions.Models;

public class Transactions : ITransactions
{
    private readonly List<Transaction> _transactions;
    public int Count { get; }

    public Transactions(List<Transaction> transactions)
    {
        _transactions = transactions;
    }

    public IEnumerator<Transaction> GetEnumerator() => _transactions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public TransactionsSummaryReport GetSummaryReport()
    {
        var netInvestedFiat = _transactions.Sum(t => t.FiatAmount);
        var totalBtcInvestment = _transactions.Sum(t => t.BtcAmount);
        var currentBtcPrice = 62000.0;
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
}