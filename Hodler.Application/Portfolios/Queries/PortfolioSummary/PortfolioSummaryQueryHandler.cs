using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Queries.PortfolioSummary;

public class PortfolioSummaryQueryHandler : IRequestHandler<PortfolioSummaryQuery, PortfolioSummaryInfo>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PortfolioSummaryQueryHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<PortfolioSummaryInfo> Handle(PortfolioSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var service = _serviceScopeFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<IPortfolioQueryService>();

        var summaryReport = await service.GetPortfolioSummaryAsync(request.UserId, cancellationToken);

        return summaryReport;
    }
}