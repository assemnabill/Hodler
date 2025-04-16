using System.Collections.ObjectModel;
using Corz.DomainDriven.Abstractions.Models.Results;
using Corz.Extensions.DateTime;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public class Transactions : ReadOnlyCollection<Transaction>, ITransactions
{
    public Transactions(IEnumerable<Transaction> transactions)
        : base(transactions.OrderBy(x => x.Timestamp).ToList())
    {
        EnsureNoDuplicates();
    }

    public SyncResult<ITransactions> Sync(IEnumerable<Transaction> transactions)
    {
        var changed = false;
        transactions = transactions.OrderBy(x => x.Timestamp).ToList();
        var currentTransactions = Items
            .OrderBy(x => x.Timestamp)
            .ToList();

        // TODO: EQUALITY CHECK NOT WORKING
        if (Equals(transactions, currentTransactions))
            return new SyncResult<ITransactions>(changed, this);

        foreach (var transaction in transactions)
        {
            if (Items.Any(x => x.Equals(transaction)))
                continue;

            currentTransactions.Add(transaction);
            changed = true;
        }

        return new SyncResult<ITransactions>(changed, new Transactions(currentTransactions));
    }

    public async Task<FiatAmount> GetPortfolioValueOnDateAsync(
        DateOnly date,
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        FiatCurrency userDisplayCurrency,
        CancellationToken cancellationToken = default
    )
    {
        var transactions = Items
            .Where(x => x.Timestamp.ToDate() <= date)
            .OrderBy(x => x.Timestamp)
            .ToList();

        if (transactions.Count == 0)
            return new FiatAmount(0, userDisplayCurrency);

        // foreach (var transaction in transactions)
        // {
        // todo: if transaction not in user display currency, convert to user display currency
        // todo: adjust when adding transaction and when display currency changed
        //
        //     if (transaction.FiatAmount.FiatCurrency.Id != userDisplayCurrency.Id)
        //     {
        //         var btcPriceOnDate = await historicalBitcoinPriceProvider
        //             .GetHistoricalPriceOnDateAsync(
        //                 userDisplayCurrency,
        //                 transaction.Timestamp.ToDate(),
        //                 cancellationToken
        //             );
        //        
        //         transaction.FiatAmount = new FiatAmount(
        //             transaction.BtcAmount.Amount * btcPriceOnDate.Amount,
        //             userDisplayCurrency
        //         );
        //     }
        // }

        var bought = transactions.Where(x => x.Type == TransactionType.Buy).Sum(x => x.BtcAmount);
        var sold = transactions.Where(x => x.Type == TransactionType.Sell).Sum(x => x.BtcAmount);
        var netBtcOnDate = bought - sold;

        // TODO: Get price on date from historical data
        var btcPriceOnDate = transactions.Last().MarketPrice;

        // TODO: Get from user settings
        var fiatCurrency = transactions.First().FiatAmount.FiatCurrency;

        return new FiatAmount(netBtcOnDate * btcPriceOnDate, fiatCurrency);
    }

    public ITransactions Add(
        PortfolioId portfolioId,
        TransactionType transactionType,
        DateTimeOffset date,
        FiatAmount fiatAmount,
        BitcoinAmount bitcoinAmount,
        CryptoExchangeName? cryptoExchange,
        out IResult result
    )
    {
        result = new SuccessResult();
        var marketPrice = new FiatAmount(fiatAmount.Amount / bitcoinAmount.Amount, fiatAmount.FiatCurrency);
        var newTransaction = new Transaction(
            portfolioId,
            transactionType,
            fiatAmount,
            bitcoinAmount,
            marketPrice,
            date,
            cryptoExchange
        );

        return Items.Contains(newTransaction) ? this : new Transactions(Items.Append(newTransaction));
    }

    public async Task<PortfolioSummaryInfo> GetSummaryReportAsync(
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(currentBitcoinPriceProvider);

        if (Items.Count == 0)
        {
            var zero = new FiatAmount(0, FiatCurrency.UsDollar);
            return new PortfolioSummaryInfo(zero, new BitcoinAmount(0), zero, zero, zero, 0, zero, zero, 0);
        }

        var netInvestedFiat = Items.Sum(t => t.FiatAmount);
        var totalBtcInvestment = Items.Sum(t => t.BtcAmount);

        // TODO: GET IN USER CURRENCY AND CONVERT TRANSACTIONS IN FORIGN CURRENCY TO USER CURRENCY
        var currentBtcPrice = await currentBitcoinPriceProvider
            .GetCurrentBitcoinPriceInAmericanDollarsAsync(cancellationToken);

        var currentValue = totalBtcInvestment * currentBtcPrice.Amount;
        var totalProfitFiat = currentValue - netInvestedFiat;
        var totalProfitPercentage = Convert.ToDouble(totalProfitFiat / netInvestedFiat * 100);

        var avgBtcPrice = Items.Average(x => x.MarketPrice);

        var taxFreeTransactions = Items
            .Where(t => t.Timestamp <= DateTimeOffset.UtcNow.AddYears(-1));

        var taxFreeTotalBtcInvestment = taxFreeTransactions.Sum(t => t.BtcAmount);
        var taxFreeProfit = taxFreeTotalBtcInvestment * currentBtcPrice.Amount;

        var fiatCurrency = Items.First().FiatAmount.FiatCurrency;

        return new PortfolioSummaryInfo(
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

    private void EnsureNoDuplicates()
    {
        var duplicates = Items.Count - Items.Distinct().Count();

        if (duplicates > 0)
        {
            throw new ArgumentException("Duplicate transactions found");
        }
    }
}