namespace Hodler.Domain.Portfolios.Models.BitcoinWallets;

public sealed class WalletAvatar
{
    public WalletIcon Icon { get; }
    public WalletColor Color { get; }

    public WalletAvatar(WalletIcon? icon = null, WalletColor? color = null)
    {
        icon ??= new WalletIcon("\uD83D\uDCB0");
        color ??= new WalletColor("#FF2196F3");

        Icon = icon;
        Color = color;
    }

    public override string ToString() => Icon.Value;
    public override bool Equals(object? obj) => obj is WalletAvatar other && Icon == other.Icon && Color == other.Color;
    public override int GetHashCode() => HashCode.Combine(Icon, Color);
}