using System.Collections.ObjectModel;
using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public class Transactions : ReadOnlyCollection<Transaction>, ITransactions
{
    public Transactions(IEnumerable<Transaction> transactions)
        : base(transactions.OrderBy(x => x.Timestamp).ToList())
    {
        EnsureNoDuplicates();
    }

    public SyncResult<ITransactions> Sync(List<Transaction> transactions)
    {
        var changed = false;
        transactions = transactions
            .OrderBy(x => x.Timestamp)
            .ToList();

        var currentTransactions = Items
            .OrderBy(x => x.Timestamp)
            .ToList();

        // TODO: EQUALITY CHECK NOT WORKING? - FIXED BY TURNING ENUMERABLE TO LIST, TEST THIS LATER
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

        var buyTransactions = Items.Where(x => x.Type == TransactionType.Buy).ToList();
        var sellTransactions = Items.Where(x => x.Type == TransactionType.Sell).ToList();

        var netInvestedFiat = buyTransactions.Sum(x => x.FiatAmount.Amount)
                              - sellTransactions.Sum(x => x.FiatAmount.Amount);

        var totalBtcInvestment = buyTransactions.Sum(x => x.BtcAmount.Amount)
                                 - sellTransactions.Sum(x => x.BtcAmount.Amount);

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