using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolio.Models;

public record Transaction(
    PortfolioId PortfolioId,
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    FiatAmount MarketPrice,
    DateTimeOffset Timestamp,
    CryptoExchangeNames? CryptoExchange = null
);