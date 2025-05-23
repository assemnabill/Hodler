using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Results;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.RemoveTransaction;

public class RemoveTransactionCommand : IRequest<IResult>
{
    public UserId UserId { get; }
    public TransactionId TransactionId { get; }

    public RemoveTransactionCommand(UserId userId, TransactionId transactionId)
    {
        UserId = userId;
        TransactionId = transactionId;

    }
}