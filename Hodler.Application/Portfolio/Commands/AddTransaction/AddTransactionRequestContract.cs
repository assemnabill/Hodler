using Hodler.Domain.Portfolio.Models;

namespace Hodler.Application.Portfolio.Commands.AddTransaction;

public class AddTransactionRequestContract
{
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public decimal Price { get; set; }
    public TransactionType Type { get; set; }
}