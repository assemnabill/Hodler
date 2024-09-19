using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.User.Models;

namespace Hodler.Application.Portfolio.Commands.SyncWithExchange.Services;

internal class PortfolioQueryService : IPortfolioQueryService
{
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly ICurrentPriceProvider _currentPriceProvider;

    public PortfolioQueryService(
        IPortfolioRepository portfolioRepository,
        ICurrentPriceProvider currentPriceProvider
    )
    {
        _portfolioRepository = portfolioRepository;
        _currentPriceProvider = currentPriceProvider;
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

        var summary = await portfolio.Transactions.GetSummaryReportAsync(_currentPriceProvider, cancellationToken)!;

        return summary;
    }

    private async Task<IPortfolio> FindOrCreatePortfolioByUserIdAsync(UserId userId, CancellationToken cancellationToken)
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