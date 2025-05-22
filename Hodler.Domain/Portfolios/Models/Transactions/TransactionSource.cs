using Corz.Extensions.Enumeration;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public record TransactionSource : ITransactionSource
{
    private TransactionSource(TransactionSourceType type, string identifier)
    {
        Type = type;
        Identifier = identifier;
    }

    public TransactionSourceType Type { get; }
    public string Identifier { get; }

    public static TransactionSource FromWallet(BitcoinWalletId walletId)
    {
        ArgumentNullException.ThrowIfNull(walletId);

        return new TransactionSource(TransactionSourceType.Wallet, walletId.Value.ToString());
    }

    public static TransactionSource FromExchange(CryptoExchangeName exchangeName) =>
        new(TransactionSourceType.CryptoExchange, exchangeName.GetDescription());
}