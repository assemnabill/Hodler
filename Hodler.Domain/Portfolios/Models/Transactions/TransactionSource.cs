using Corz.Extensions.Enumeration;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public record TransactionSource : ITransactionSource
{
    private TransactionSource(TransactionSourceType type, string identifier, string? name = null)
    {
        Type = type;
        Identifier = identifier;
        Name = name;
    }

    public TransactionSourceType Type { get; }
    public string Identifier { get; }
    public string? Name { get; }

    public static TransactionSource FromWallet(BitcoinWalletId walletId, string? name = null)
    {
        ArgumentNullException.ThrowIfNull(walletId);

        return new TransactionSource(
            TransactionSourceType.Wallet,
            walletId.Value.ToString(),
            name
        );
    }

    public static TransactionSource FromExchange(CryptoExchangeName exchangeName, string? name = null) =>
        new(
            TransactionSourceType.CryptoExchange,
            $"{(int)exchangeName}",
            name ?? exchangeName.GetDescription()
        );
}