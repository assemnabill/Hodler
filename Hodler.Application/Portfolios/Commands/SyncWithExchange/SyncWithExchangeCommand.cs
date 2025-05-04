using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Commands.SyncWithExchange;

public class SyncWithExchangeCommand : IRequest<PortfolioInfo>
{
    public SyncWithExchangeCommand(UserId userId, CryptoExchangeName cryptoExchangeName)
    {
        UserId = userId;
        CryptoExchangeName = cryptoExchangeName;
    }

    public CryptoExchangeName CryptoExchangeName { get; }
    public UserId UserId { get; }
}