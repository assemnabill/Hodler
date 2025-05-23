using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Shared.Results;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.ModifyTransaction;

public class ModifyTransactionCommand : IRequest<IResult>
{
    public TransactionId TransactionId { get; }
    public UserId UserId { get; }
    public DateTimeOffset Timestamp { get; }
    public BitcoinAmount Amount { get; }
    public FiatAmount Price { get; }
    public TransactionType Type { get; }
    public ITransactionSource? TransactionSource { get; }

    public ModifyTransactionCommand(
        TransactionId transactionId,
        UserId userId,
        DateTimeOffset timestamp,
        BitcoinAmount amount,
        FiatAmount price,
        TransactionType type,
        ITransactionSource? transactionSource
    )
    {
        ArgumentNullException.ThrowIfNull(transactionId);
        ArgumentNullException.ThrowIfNull(userId);

        TransactionId = transactionId;
        UserId = userId;
        Timestamp = timestamp;
        Amount = amount;
        Price = price;
        Type = type;
        TransactionSource = transactionSource;
    }
}