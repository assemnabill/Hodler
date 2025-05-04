using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolios.Models;

namespace Hodler.Contracts.Portfolios.AddTransaction;

public class AddTransactionRequestContract
{
    public DateTime Date { get; set; }
    public decimal BitcoinAmount { get; set; }
    public FiatAmountDto FiatAmount { get; set; }
    public TransactionType Type { get; set; }
    public CryptoExchangeNames? CryptoExchange { get; set; }
}