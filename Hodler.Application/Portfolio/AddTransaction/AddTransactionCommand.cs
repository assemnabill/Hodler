using Hodler.Domain.Portfolio.Models;
using MediatR;

namespace Hodler.Application.Portfolio.AddTransaction;

public class AddTransactionCommand : IRequest
{
    public AddTransactionCommand(Guid userId,
        DateTime date,
        decimal amount,
        decimal price,
        TransactionType type)
    {
        // TODO: Validation        
        
        UserId = userId;
        Date = date;
        Amount = amount;
        Price = price;
        Type = type;
    }

    public Guid UserId { get; }
    public DateTime Date { get; }
    public decimal Amount { get; }
    public decimal Price { get; }
    public TransactionType Type { get; }
}