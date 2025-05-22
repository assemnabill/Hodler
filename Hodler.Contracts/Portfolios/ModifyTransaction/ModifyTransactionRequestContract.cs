using Hodler.Contracts.Portfolios.AddTransaction;
using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolios.Models.Transactions;

namespace Hodler.Contracts.Portfolios.ModifyTransaction;

public class ModifyTransactionRequestContract
{
    public DateTime Date { get; set; }
    public decimal BitcoinAmount { get; set; }
    public FiatAmountDto FiatAmount { get; set; }
    public TransactionType Type { get; set; }
    public TransactionSourceDto? TransactionSource { get; set; }
}