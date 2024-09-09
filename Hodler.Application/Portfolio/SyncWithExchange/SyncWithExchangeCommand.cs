using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using MediatR;

namespace Hodler.Application.Portfolio.SyncWithExchange;

public class SyncWithExchangeCommand : IRequest<IPortfolio>
{
    public CryptoExchange ExchangeName { get; }
    public Guid UserId { get; }

    public SyncWithExchangeCommand(Guid userId, CryptoExchange exchangeName)
    {
        UserId = userId;
        ExchangeName = exchangeName;
    }
}