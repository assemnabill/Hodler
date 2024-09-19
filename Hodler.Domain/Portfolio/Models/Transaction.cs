using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public record Transaction(
    PortfolioId PortfolioId,
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    double MarketPrice,
    DateTimeOffset Timestamp,
    CryptoExchange? CryptoExchange = null);