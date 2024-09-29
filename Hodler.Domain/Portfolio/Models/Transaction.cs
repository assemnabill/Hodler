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
    CryptoExchangeNames? CryptoExchange = null)
{
    public virtual bool Equals(Transaction? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return PortfolioId.Equals(other.PortfolioId) && Type == other.Type && FiatAmount.Equals(other.FiatAmount) &&
               BtcAmount.Equals(other.BtcAmount) && MarketPrice.Equals(other.MarketPrice) &&
               Timestamp.Equals(other.Timestamp) && CryptoExchange == other.CryptoExchange;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PortfolioId, (int)Type, FiatAmount, BtcAmount, MarketPrice, Timestamp, CryptoExchange);
    }
}