using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.PriceCatalog.Services;
using Hodler.Domain.User.Models;

namespace Hodler.Application.Portfolio.Services;

internal class PortfolioQueryService : IPortfolioQueryService
{
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly ICurrentBitcoinPriceProvider _currentBitcoinPriceProvider;

    public PortfolioQueryService(
        IPortfolioRepository portfolioRepository,
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider
    )
    {
        _portfolioRepository = portfolioRepository;
        _currentBitcoinPriceProvider = currentBitcoinPriceProvider;
    }

    public async Task<IPortfolio> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var portfolio = await FindOrCreatePortfolioByUserIdAsync(userId, cancellationToken);

        return portfolio;
    }

    public async Task<PortfolioSummary> GetPortfolioSummaryAsync(UserId userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var portfolio = await GetByUserIdAsync(userId, cancellationToken);

        var summary = await portfolio.Transactions
            .GetSummaryReportAsync(_currentBitcoinPriceProvider, cancellationToken)!;

        return summary;
    }

    private async Task<IPortfolio> FindOrCreatePortfolioByUserIdAsync(
        UserId userId,
        CancellationToken cancellationToken)
    {
        var portfolio = await _portfolioRepository.FindByAsync(userId, cancellationToken);

        if (portfolio is null)
        {
            portfolio = Domain.Portfolio.Models.Portfolio.Create(userId);
            await _portfolioRepository.StoreAsync(portfolio, cancellationToken);
        }

        return portfolio;
    }
}