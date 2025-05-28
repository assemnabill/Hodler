using Hodler.Domain.Portfolios.Models.Transactions;

namespace Hodler.Contracts.Portfolios.AddTransaction;

public class TransactionSourceDto
{
    /// <summary>
    ///  Transaction source type
    /// </summary>
    public TransactionSourceType Type { get; set; }

    /// <summary>
    /// Wallet id or crypto exchange id
    /// </summary>
    public string? Identifier { get; set; }

    /// <summary>
    /// Wallet name or crypto exchange name
    /// </summary>
    public string? Name { get; set; }
}