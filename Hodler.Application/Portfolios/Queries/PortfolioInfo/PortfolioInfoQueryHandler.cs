using Hodler.Domain.Portfolios.Services;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Queries.PortfolioInfo;

public class PortfolioInfoQueryHandler
    : IRequestHandler<PortfolioInfoQuery, Domain.Portfolios.Models.PortfolioInfo>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PortfolioInfoQueryHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<Domain.Portfolios.Models.PortfolioInfo> Handle(
        PortfolioInfoQuery request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var service = _serviceScopeFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IPortfolioQueryService>();

        var portfolio = await service.FindOrCreatePortfolioAsync(request.UserId, cancellationToken);

        var portfolioInfo = portfolio.Adapt<Domain.Portfolios.Models.PortfolioInfo>();

        return portfolioInfo;
    }
}