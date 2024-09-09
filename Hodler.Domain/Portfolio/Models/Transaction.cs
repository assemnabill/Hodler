namespace Hodler.Domain.Portfolio.Models;

public record Transaction(
    TransactionType Type,
    double FiatAmount,
    double BtcAmount,
    double MarketPrice,
    DateTimeOffset Timestamp);