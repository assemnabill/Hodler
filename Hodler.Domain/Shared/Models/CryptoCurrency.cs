using Hodler.Domain.Shared.Aggregate;

namespace Hodler.Domain.Shared.Models;

public class CryptoCurrency : TypeSafeEnum<CryptoCurrency>
{
    public static readonly CryptoCurrency Bitcoin = new(1, "₿", "BTC");

    public string Symbol { get; }
    public string Ticker { get; }

    private CryptoCurrency(int id, string symbol, string ticker) : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(symbol);

        Symbol = symbol;
        Ticker = ticker;
    }
}