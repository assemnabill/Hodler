using Hodler.Domain.Portfolio.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolio.Queries.PortfolioSummary;

public class PortfolioSummaryQueryHandler : IRequestHandler<PortfolioSummaryQuery, Domain.Portfolio.Models.PortfolioSummary>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PortfolioSummaryQueryHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<Domain.Portfolio.Models.PortfolioSummary> Handle(PortfolioSummaryQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var service = _serviceScopeFactory
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<ITransactionsQueryService>();

        var summaryReport = await service.GetPortfolioSummaryAsync(request.UserId, cancellationToken);
        
        return summaryReport;
    }
}