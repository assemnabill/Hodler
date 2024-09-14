using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.Shared.Models;

public class CryptoCurrency : TypeSafeEnum<CryptoCurrency>
{
    public string Symbol { get; }

    public static readonly CryptoCurrency Bitcoin = new(1, "BTC");

    private CryptoCurrency(int id, string symbol) : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(symbol);

        Symbol = symbol;
    }
}