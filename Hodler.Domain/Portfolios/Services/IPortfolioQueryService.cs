using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
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

    Task<PortfolioValueChartInfo> CalculatePortfolioValueChartAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );

    Task<IPortfolio?> FindPortfolioAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<IBitcoinWallet>> RetrieveBitcoinWalletsAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );
}