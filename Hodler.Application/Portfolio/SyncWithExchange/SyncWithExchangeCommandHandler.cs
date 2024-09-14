using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Services.Sync;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolio.SyncWithExchange;

public class SyncWithExchangeCommandHandler : IRequestHandler<SyncWithExchangeCommand, IPortfolio>
{
    private readonly IServiceProvider _serviceProvider;

    public SyncWithExchangeCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<IPortfolio> Handle(
        SyncWithExchangeCommand request,
        CancellationToken cancellationToken)
    {
        var domainService = _serviceProvider
            .GetRequiredKeyedService<IPortfolioSyncService>(request.ExchangeName.Name);

        return domainService.SyncWithExchangeAsync(request.UserId, cancellationToken);
    }
}