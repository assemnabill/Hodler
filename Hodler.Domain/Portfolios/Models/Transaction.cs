using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Portfolios.Models;

public record Transaction(
    PortfolioId PortfolioId,
    TransactionId Id,
    TransactionType Type,
    FiatAmount FiatAmount,
    BitcoinAmount BtcAmount,
    DateTimeOffset Timestamp,
    FiatAmount MarketPrice,
    CryptoExchangeName? CryptoExchange = null
)
{
    public bool IsInCurrency(FiatCurrency fiatCurrency) => FiatAmount.FiatCurrency.Id == fiatCurrency.Id;
}