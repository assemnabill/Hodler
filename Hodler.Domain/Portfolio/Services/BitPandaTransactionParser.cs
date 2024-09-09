using System.Globalization;
using Hodler.Domain.Portfolio.Models;

namespace Hodler.Domain.Portfolio.Services;

public class BitPandaTransactionParser : IBitPandaTransactionParser
{
    public ITransactions ParseTransactions(IEnumerable<string[]> lines) =>
        new Transactions(
            lines
                .Select(ParseTransaction)
                .Where(x => x is not null)
                .ToList()!
        );

    public Transaction? ParseTransaction(string[] line)
    {
        var transactionType = line[2].ToLower() switch
        {
            "buy" => TransactionType.Buy,
            "sell" => TransactionType.Sell,
            _ => TransactionType.Unknown
        };

        if (transactionType is TransactionType.Unknown)
        {
            return null;
        }

        var fiatAmount = double.Parse(line[4], NumberStyles.Float, CultureInfo.InvariantCulture);
        var btcAmount = double.Parse(line[6], NumberStyles.Float, CultureInfo.InvariantCulture);
        var marketPrice = double.Parse(line[8], NumberStyles.Float, CultureInfo.InvariantCulture);
        var timestamp = DateTimeOffset.Parse(line[1]);

        return new Transaction(transactionType, fiatAmount, btcAmount, marketPrice, timestamp);
    }
}