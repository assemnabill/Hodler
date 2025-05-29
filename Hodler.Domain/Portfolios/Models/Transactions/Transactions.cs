using System.Collections.ObjectModel;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public class Transactions : ReadOnlyCollection<Transaction>, ITransactions
{
    public ITransactions BuyTransactions => new Transactions(Items.Where(x => x.Type == TransactionType.Buy));
    public ITransactions SellTransactions => new Transactions(Items.Where(x => x.Type == TransactionType.Sell));
    public ITransactions SentTransactions => new Transactions(Items.Where(x => x.Type == TransactionType.Sent));
    public ITransactions ReceivedTransactions => new Transactions(Items.Where(x => x.Type == TransactionType.Received));

    public BitcoinAmount NetBitcoinAmount =>
        BuyTransactions.Sum(x => x.BtcAmount)
        + ReceivedTransactions.Sum(x => x.BtcAmount)
        - SellTransactions.Sum(x => x.BtcAmount)
        - SentTransactions.Sum(x => x.BtcAmount);


    public Transactions(IEnumerable<Transaction> transactions)
        : base(EnsureIsValid(transactions))
    {
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

        var netInvestedFiat = BuyTransactions.Sum(x => x.FiatAmount.Amount)
                              - SellTransactions.Sum(x => x.FiatAmount.Amount);

        var currentValue = NetBitcoinAmount * currentBtcPriceInUsd.Amount;
        var totalProfitFiat = currentValue - netInvestedFiat;
        var totalProfitPercentage = netInvestedFiat > 0
            ? Convert.ToDouble(totalProfitFiat / netInvestedFiat * 100)
            : 0.0;

        // todo: either all in usd or convert to user currency
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
            NetBitcoinAmount,
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
                       && x.Type == newTransaction.Type
                       && x.TransactionSource == newTransaction.TransactionSource
                       && x.TransactionFee == newTransaction.TransactionFee
        );

    private static IList<Transaction> EnsureIsValid(IEnumerable<Transaction> transactions)
    {
        var items = transactions.ToList();

        EnsureNoDuplicates(items);
        EnsureAllHaveSamePortfolioId(items);

        return items
            .OrderByDescending(x => x.Timestamp)
            .ToList();
    }

    private static void EnsureAllHaveSamePortfolioId(List<Transaction> transactions)
    {

        if (transactions.Count == 0)
            return;

        var portfolioId = transactions.First().PortfolioId;

        if (transactions.Any(transaction => transaction.PortfolioId != portfolioId))
            throw new ArgumentException("All transactions must have the same PortfolioId");
    }


    private static void EnsureNoDuplicates(List<Transaction> transactions)
    {
        var duplicates = transactions.Count - transactions.Distinct().Count();

        if (duplicates > 0)
        {
            throw new ArgumentException("Duplicate transactions found");
        }
    }
}