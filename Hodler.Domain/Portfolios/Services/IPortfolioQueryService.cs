using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Services;

public interface IPortfolioQueryService
{
    Task<IPortfolio> FindOrCreatePortfolioAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );

    Task<PortfolioSummaryInfo> GetPortfolioSummaryAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<ChartSpot>> CalculatePortfolioValueChartAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );
}