using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.AddTransaction;

public class AddTransactionCommand : IRequest
{
    public Guid UserId { get; }
    public DateTime Date { get; }
    public decimal Amount { get; }
    public decimal Price { get; }
    public TransactionType Type { get; }

    public AddTransactionCommand(
        UserId userId,
        DateTime date,
        decimal amount,
        decimal price,
        TransactionType type
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
        Date = date;
        Amount = amount;
        Price = price;
        Type = type;
    }
}