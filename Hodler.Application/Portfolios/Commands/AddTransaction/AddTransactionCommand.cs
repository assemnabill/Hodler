using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.AddTransaction;

public class AddTransactionCommand : IRequest<IResult>
{
    public UserId UserId { get; }
    public DateTime Date { get; }
    public BitcoinAmount Amount { get; }
    public FiatAmount Price { get; }
    public TransactionType Type { get; }
    public CryptoExchangeName? CryptoExchange { get; }

    public AddTransactionCommand(
        UserId userId,
        DateTime date,
        BitcoinAmount amount,
        FiatAmount price,
        TransactionType type,
        CryptoExchangeName? cryptoExchange
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
        Date = date;
        Amount = amount;
        Price = price;
        Type = type;
        CryptoExchange = cryptoExchange;
    }
}