using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.SyncWithExchange;

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