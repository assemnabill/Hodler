namespace Hodler.Domain.Transactions.Models;

public record Transaction(
    TransactionType Type,
    double FiatAmount,
    double BtcAmount,
    double MarketPrice,
    DateTimeOffset Timestamp);