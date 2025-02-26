namespace Hodler.Contracts.Portfolio.AddTransaction;

public class AddTransactionRequestContract
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public TransactionType Type { get; set; }
}