using System.Collections.ObjectModel;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public class Transactions : ReadOnlyCollection<Transaction>, ITransactions
{
    public Transactions(IEnumerable<Transaction> transactions)
        : base(transactions.OrderBy(x => x.Timestamp).ToList())
    {
        EnsureNoDuplicates();
        EnsureAllHaveSamePortfolioId();
    }


    public SyncResult<ITransactions> Sync(List<Transaction> newTransactions)
    {
        var changed = false;
        newTransactions = newTransactions
            .OrderBy(x => x.Timestamp)
            .ToList();

        var currentTransactions = Items
            .OrderBy(x => x.Timestamp)
            .ToList();

        foreach (var transaction in newTransactions.Where(transaction => !AlreadyExists(transaction)))
        {
            currentTransactions.Add(transaction);
            changed = true;
        }

        return new SyncResult<ITransactions>(changed, changed ? new Transactions(currentTransactions) : this);
    }

    public ITransactions Remove(TransactionId transactionId) => new Transactions(Items.Where(x => x.Id != transactionId));

    public async Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(currentBitcoinPriceProvider);

        if (Items.Count == 0)
            return PortfolioSummaryInfo.Empty;

        var currentBtcPriceInUsd = await currentBitcoinPriceProvider
            .GetCurrentBitcoinPriceInAmericanDollarsAsync(cancellationToken);

        var transactions = Items
            .Select(x => x.IsInCurrency(FiatCurrency.UsDollar)
                ? x
                : x with { FiatAmount = x.FiatAmount.ConvertTo(FiatCurrency.UsDollar) }
            )
            .ToList();

        var buyTransactions = transactions.Where(x => x.Type == TransactionType.Buy).ToList();
        var sellTransactions = transactions.Where(x => x.Type == TransactionType.Sell).ToList();

        var netInvestedFiat = buyTransactions.Sum(x => x.FiatAmount.Amount)
                              - sellTransactions.Sum(x => x.FiatAmount.Amount);

        var netInvestedBtc = buyTransactions.Sum(x => x.BtcAmount.Amount)
                             - sellTransactions.Sum(x => x.BtcAmount.Amount);

        var currentValue = netInvestedBtc * currentBtcPriceInUsd.Amount;
        var totalProfitFiat = currentValue - netInvestedFiat;
        var totalProfitPercentage = Convert.ToDouble(totalProfitFiat / netInvestedFiat * 100);
        var avgBtcPrice = transactions.Average(x => x.MarketPrice);

        // todo: this is only true for germany, fix this for other countries
        var taxFreeTotalBtcInvestment = transactions
            .Where(t => t.Timestamp <= DateTimeOffset.UtcNow.AddYears(-1))
            .Sum(t => t.BtcAmount);

        var taxFreeProfit = taxFreeTotalBtcInvestment * currentBtcPriceInUsd.Amount;

        // todo: replace with user currency
        var fiatCurrency = transactions.First().FiatAmount.FiatCurrency;

        return new PortfolioSummaryInfo(
            new FiatAmount(netInvestedFiat, fiatCurrency),
            netInvestedBtc,
            currentBtcPriceInUsd,
            new FiatAmount(currentValue, fiatCurrency),
            new FiatAmount(totalProfitFiat, fiatCurrency),
            totalProfitPercentage,
            new FiatAmount(avgBtcPrice, fiatCurrency),
            new FiatAmount(taxFreeProfit, fiatCurrency),
            Convert.ToDouble(taxFreeTotalBtcInvestment)
        );
    }

    public bool AlreadyExists(Transaction newTransaction) =>
        Items.Any(x => x.Timestamp == newTransaction.Timestamp
                       && x.FiatAmount == newTransaction.FiatAmount
                       && x.BtcAmount == newTransaction.BtcAmount
                       && x.Type == newTransaction.Type);

    private void EnsureAllHaveSamePortfolioId()
    {
        if (Items.Count == 0)
            return;

        var portfolioId = Items.First().PortfolioId;

        if (Items.Any(transaction => transaction.PortfolioId != portfolioId))
            throw new ArgumentException("All transactions must have the same PortfolioId");
    }


    private void EnsureNoDuplicates()
    {
        var duplicates = Items.Count - Items.Distinct().Count();

        if (duplicates > 0)
        {
            throw new ArgumentException("Duplicate transactions found");
        }
    }
}