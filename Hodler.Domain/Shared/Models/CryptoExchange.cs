using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.Shared.Models;

public class CryptoExchange : TypeSafeEnum<CryptoExchange>
{
    public string Name { get; }

    public static readonly CryptoExchange BitPanda = new(1, "BitPanda");
    public static readonly CryptoExchange Kraken = new(2, "Kraken");

    public CryptoExchange(int id, string name) : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name;
    }
}