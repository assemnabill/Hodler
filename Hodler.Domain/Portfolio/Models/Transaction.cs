using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public record Transaction(
    Guid TransactionId,
    TransactionType Type,
    FiatCurrency FiatCurrency,
    double FiatAmount,
    double BtcAmount,
    double MarketPrice,
    DateTimeOffset Timestamp,
    CryptoExchange? CryptoExchange = null);