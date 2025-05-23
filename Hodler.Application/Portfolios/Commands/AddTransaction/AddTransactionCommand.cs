using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.AddTransaction;

public class AddTransactionCommand : IRequest<IResult>
{
    public UserId UserId { get; }
    public DateTimeOffset Timestamp { get; }
    public BitcoinAmount Amount { get; }
    public FiatAmount Price { get; }
    public TransactionType Type { get; }
    public ITransactionSource? TransactionSource { get; }

    public AddTransactionCommand(
        UserId userId,
        DateTimeOffset timestamp,
        BitcoinAmount amount,
        FiatAmount price,
        TransactionType type,
        ITransactionSource? transactionSource
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
        Timestamp = timestamp;
        Amount = amount;
        Price = price;
        Type = type;
        TransactionSource = transactionSource;
    }
}