using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.ModifyTransaction;

public class ModifyTransactionCommand : IRequest<IResult>
{
    public TransactionId TransactionId { get; }
    public UserId UserId { get; }
    public DateTime Date { get; }
    public BitcoinAmount Amount { get; }
    public FiatAmount Price { get; }
    public TransactionType Type { get; }
    public CryptoExchangeName? CryptoExchange { get; }

    public ModifyTransactionCommand(
        TransactionId transactionId,
        UserId userId,
        DateTime date,
        BitcoinAmount amount,
        FiatAmount price,
        TransactionType type,
        CryptoExchangeName? cryptoExchange
    )
    {
        ArgumentNullException.ThrowIfNull(transactionId);
        ArgumentNullException.ThrowIfNull(userId);

        TransactionId = transactionId;
        UserId = userId;
        Date = date;
        Amount = amount;
        Price = price;
        Type = type;
        CryptoExchange = cryptoExchange;
    }
}