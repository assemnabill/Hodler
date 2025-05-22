using System.ComponentModel;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public enum TransactionSourceType
{
    [Description("Wallet")] Wallet,
    [Description("Crypto Exchange")] CryptoExchange
}