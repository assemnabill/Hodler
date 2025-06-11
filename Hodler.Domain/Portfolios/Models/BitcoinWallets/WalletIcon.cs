using System.Globalization;

namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public sealed class WalletIcon
{
    public string Value { get; }

    public WalletIcon(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Icon must not be empty.");

        // Basic emoji validation: must be a single Unicode grapheme cluster
        var textEnum = StringInfo.GetTextElementEnumerator(value);
        int count = 0;

        while (textEnum.MoveNext())
            count++;

        if (count != 1)
            throw new ArgumentException("Icon must be a single emoji.");

        Value = value;
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is WalletIcon other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}