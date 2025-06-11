namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public sealed class WalletName
{
    public string Value { get; }

    public WalletName(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 24)
            throw new ArgumentException("Wallet name must be 1-24 characters.");

        Value = value;
    }

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is WalletName other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}