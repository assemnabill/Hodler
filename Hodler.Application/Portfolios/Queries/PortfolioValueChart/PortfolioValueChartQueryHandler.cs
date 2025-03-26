using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Queries.PortfolioValueChart;

public class PortfolioValueChartQueryHandler
    : IRequestHandler<PortfolioValueChartQuery, IReadOnlyCollection<ChartCandle>>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PortfolioValueChartQueryHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<IReadOnlyCollection<ChartCandle>> Handle(
        PortfolioValueChartQuery request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var service = _serviceScopeFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IPortfolioQueryService>();

        var portfolioValueChart = await service.CalculatePortfolioValueChartAsync(request.UserId, cancellationToken);

        return portfolioValueChart;
    }
}