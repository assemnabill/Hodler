using System.Globalization;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Services;

public interface IKrakenTransactionParser : ITransactionParser;

public class KrakenTransactionParser : IKrakenTransactionParser
{
    public ITransactions ParseTransactions(IEnumerable<string[]> lines)
    {
        var transactions = lines
            .Select(ParseKrakenTransactionRow)
            .Where(x => x.KrakenTransactionType.Equals("spend") || x.KrakenTransactionType.Equals("receive"))
            .GroupBy(x => x.ReferenceId)
            .Select(x =>
            {
                var spendingTransaction = x.FirstOrDefault(x => x.KrakenTransactionType.Equals("spend"));
                var receivingTransaction = x.FirstOrDefault(x => x.KrakenTransactionType.Equals("receive"));
                var marketPrice = Math.Abs(spendingTransaction!.Amount / receivingTransaction!.Amount);

                return new Transaction(
                    Guid.NewGuid(), 
                    TransactionType.Buy,
                    FiatCurrency.Euro,
                    spendingTransaction.Amount,
                    receivingTransaction.Amount,
                    marketPrice,
                    spendingTransaction.Timestamp,
                    CryptoExchange.Kraken
                );
            })
            .ToList();

        return new Transactions(transactions);
    }

    public Transaction? ParseTransaction(string[] line) => throw new NotImplementedException();


    private static KrakenTransactionRow ParseKrakenTransactionRow(string[] line)
    {
        const string dateFormat = "yyyy-MM-dd HH:mm:ss";

        return new KrakenTransactionRow(
            line[3].Trim('"'),
            Math.Abs(double.Parse(line[8], NumberStyles.Float, CultureInfo.InvariantCulture)),
            DateTimeOffset.ParseExact(line[2].Trim('"'), dateFormat, CultureInfo.InvariantCulture),
            line[1].Trim('"')
        );
    }
}

public record KrakenTransactionRow
{
    public string KrakenTransactionType { get; }
    public double Amount { get; }
    public DateTimeOffset Timestamp { get; }
    public string ReferenceId { get; }

    public KrakenTransactionRow(string krakenTransactionType, double amount, DateTimeOffset timestamp, string referenceId)
    {
        KrakenTransactionType = krakenTransactionType;
        Amount = amount;
        Timestamp = timestamp;
        ReferenceId = referenceId;
    }
}