using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Services;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolio.Queries.PortfolioByUserId;

public class PortfolioByUserIdQueryHandler
    : IRequestHandler<PortfolioByUserIdQuery, PortfolioInfo>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PortfolioByUserIdQueryHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<PortfolioInfo> Handle(PortfolioByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var service = _serviceScopeFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IPortfolioQueryService>();

        var portfolio = await service.GetByUserIdAsync(request.UserId, cancellationToken);

        var portfolioInfo = portfolio.Adapt<PortfolioInfo>();

        return portfolioInfo;
    }
}