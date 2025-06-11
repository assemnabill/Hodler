using System.Text.RegularExpressions;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public sealed class WalletColor
{
    public string Value { get; }

    public WalletColor(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Color must not be empty.");

        // Accept #RRGGBB or #AARRGGBB hex color
        if (!Regex.IsMatch(value, "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{8})$"))
            throw new ArgumentException("Color must be a valid hex string (#RRGGBB or #AARRGGBB).");

        Value = value.ToUpperInvariant();
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is WalletColor other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}