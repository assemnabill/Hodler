using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;
using MediatR;

namespace Hodler.Application.Portfolio.Commands.SyncWithExchange;

public class SyncWithExchangeCommand : IRequest<PortfolioInfo>
{
    public CryptoExchange CryptoExchange { get; }
    public UserId UserId { get; }

    public SyncWithExchangeCommand(UserId userId, CryptoExchange cryptoExchange)
    {
        UserId = userId;
        CryptoExchange = cryptoExchange;
    }
}