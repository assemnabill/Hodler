using Hodler.Domain.Shared.Aggregate;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.BitcoinPrices.Models;

public class BitcoinPrice : AggregateRoot<BitcoinPrice>, IBitcoinPrice
{
    public BitcoinPrice(
        DateOnly date,
        FiatCurrency currency,
        FiatAmount close,
        FiatAmount? open = null,
        FiatAmount? high = null,
        FiatAmount? low = null,
        FiatAmount? volume = null
    )
    {
        ArgumentNullException.ThrowIfNull(currency);
        ArgumentNullException.ThrowIfNull(close);
        ArgumentOutOfRangeException.ThrowIfNegative(close.Amount);

        Date = date;
        Currency = currency;
        Close = close;
        Open = open;
        High = high;
        Low = low;
        Volume = volume;
    }

    public DateOnly Date { get; }
    public FiatCurrency Currency { get; }
    public FiatAmount Close { get; }
    public FiatAmount? Open { get; }
    public FiatAmount? High { get; }
    public FiatAmount? Low { get; }
    public FiatAmount? Volume { get; }

    public FiatAmount Price => new(Close, Currency);

    public bool Equals(IBitcoinPrice? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Date.Equals(other.Date) &&
               Currency.Equals(other.Currency) &&
               Close.Equals(other.Close) &&
               Open == other.Open &&
               High == other.High &&
               Low == other.Low &&
               Volume == other.Volume;
    }
}