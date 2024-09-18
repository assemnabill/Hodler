using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;
using MediatR;

namespace Hodler.Application.Portfolio.Commands.SyncWithExchange;

public class SyncWithExchangeCommand : IRequest<IPortfolio>
{
    public CryptoExchange ExchangeName { get; }
    public UserId UserId { get; }

    public SyncWithExchangeCommand(UserId userId, CryptoExchange exchangeName)
    {
        UserId = userId;
        ExchangeName = exchangeName;
    }
}