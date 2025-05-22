namespace Hodler.Domain.Portfolios.Models.Transactions;

public interface ITransactionSource
{
    TransactionSourceType Type { get; }
    string Identifier { get; }
}