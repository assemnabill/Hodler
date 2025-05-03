using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.Portfolios.Models;
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