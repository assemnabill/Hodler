using System.Collections;
using Hodler.Domain.PriceCatalog.Services;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public class Transactions : ReadOnlyCollectionBase, ITransactions
{
    private readonly IReadOnlyCollection<Transaction> _transactions;

    public Transactions(IEnumerable<Transaction> transactions)
    {
        _transactions = transactions
            .OrderBy(x => x.Timestamp)
            .ToList();

        EnsureNoDuplicates();
    }

    private void EnsureNoDuplicates()
    {
        var duplicates = _transactions.Count - _transactions.Distinct().Count();

        if (duplicates > 0)
        {
            throw new ArgumentException("Duplicate transactions found");
        }
    }

    public override IEnumerator<Transaction> GetEnumerator() => _transactions.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public SyncResult<ITransactions> Sync(IEnumerable<Transaction> transactions)
    {
        var changed = false;
        transactions = transactions.OrderBy(x => x.Timestamp).ToList();
        var currentTransactions = _transactions
            .OrderBy(x => x.Timestamp)
            .ToList();

        // TODO: EQUALITY CHECK NOT WORKING
        if (Equals(transactions, currentTransactions))
            return new SyncResult<ITransactions>(changed, this);
       
        foreach (var transaction in transactions)
        {
            if (_transactions.Any(x => x.Equals(transaction)))
                continue;

            currentTransactions.Add(transaction);
            changed = true;
        }

        return new SyncResult<ITransactions>(changed, new Transactions(currentTransactions));
    }

    public async Task<PortfolioSummary> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(currentBitcoinPriceProvider);

        if (_transactions.Count == 0)
        {
            var zero = new FiatAmount(0, FiatCurrency.UsDollar);
            return new PortfolioSummary(zero, new BitcoinAmount(0), zero, zero, zero, 0, zero, zero, 0);
        }

        var netInvestedFiat = _transactions.Sum(t => t.FiatAmount);
        var totalBtcInvestment = _transactions.Sum(t => t.BtcAmount);

        // TODO: GET IN USER CURRENCY AND CONVERT TRANSACTIONS IN FORIGN CURRENCY TO USER CURRENCY
        var currentBtcPrice = await currentBitcoinPriceProvider
            .GetCurrentBitcoinPriceInAmericanDollarsAsync(cancellationToken);

        var currentValue = totalBtcInvestment * currentBtcPrice.Amount;
        var totalProfitFiat = currentValue - netInvestedFiat;
        var totalProfitPercentage = Convert.ToDouble(totalProfitFiat / netInvestedFiat * 100);

        var avgBtcPrice = _transactions.Average(x => x.MarketPrice);

        var taxFreeTransactions = _transactions
            .Where(t => t.Timestamp <= DateTimeOffset.UtcNow.AddYears(-1));

        var taxFreeTotalBtcInvestment = taxFreeTransactions.Sum(t => t.BtcAmount);
        var taxFreeProfit = taxFreeTotalBtcInvestment * currentBtcPrice.Amount;

        var fiatCurrency = _transactions.First().FiatAmount.FiatCurrency;

        return new PortfolioSummary(
            new FiatAmount(netInvestedFiat, fiatCurrency),
            totalBtcInvestment,
            currentBtcPrice,
            new FiatAmount(currentValue, fiatCurrency),
            new FiatAmount(totalProfitFiat, fiatCurrency),
            totalProfitPercentage,
            new FiatAmount(avgBtcPrice, fiatCurrency),
            new FiatAmount(taxFreeProfit, fiatCurrency),
            Convert.ToDouble(taxFreeTotalBtcInvestment)
        );
    }
}