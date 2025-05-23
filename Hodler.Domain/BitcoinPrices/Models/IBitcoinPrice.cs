using Hodler.Domain.Shared.Aggregate;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Models;

public interface IBitcoinPrice
    : IAggregateRoot<IBitcoinPrice>, IEquatable<IBitcoinPrice>
{
    DateOnly Date { get; }
    FiatCurrency Currency { get; }
    FiatAmount Price { get; }
    FiatAmount? Open { get; }
    FiatAmount? High { get; }
    FiatAmount? Low { get; }
    FiatAmount Close { get; }
    FiatAmount? Volume { get; }
}