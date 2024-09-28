using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Services;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolio.Commands.SyncWithExchange;

public class SyncWithExchangeCommandHandler : IRequestHandler<SyncWithExchangeCommand, PortfolioInfo>
{
    private readonly IServiceProvider _serviceProvider;

    public SyncWithExchangeCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<PortfolioInfo> Handle(
        SyncWithExchangeCommand request,
        CancellationToken cancellationToken)
    {
        var domainService = _serviceProvider
            .GetRequiredKeyedService<IPortfolioSyncService>(request.CryptoExchangeNames);

        var portfolio = await domainService.SyncWithExchangeAsync(request.UserId, cancellationToken);
        
        var result = portfolio.Adapt<PortfolioInfo>();
        
        return result;
    }
}