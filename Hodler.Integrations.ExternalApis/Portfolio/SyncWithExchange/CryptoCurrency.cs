using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Integrations.ExternalApis.Portfolio.SyncWithExchange;

public class CryptoCurrency : TypeSafeEnum<CryptoCurrency>
{
    public string Symbol { get; }

    public static readonly CryptoCurrency Bitcoin = new(1, "BTC");

    public CryptoCurrency(int id, string symbol) : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(symbol);
        
        Symbol = symbol;
    }
}