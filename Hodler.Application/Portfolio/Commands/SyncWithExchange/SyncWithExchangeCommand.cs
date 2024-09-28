using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;
using MediatR;

namespace Hodler.Application.Portfolio.Commands.SyncWithExchange;

public class SyncWithExchangeCommand : IRequest<PortfolioInfo>
{
    public CryptoExchangeNames CryptoExchangeNames { get; }
    public UserId UserId { get; }

    public SyncWithExchangeCommand(UserId userId, CryptoExchangeNames cryptoExchangeNames)
    {
        UserId = userId;
        CryptoExchangeNames = cryptoExchangeNames;
    }
}