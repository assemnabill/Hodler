using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.PriceCatalogs.Ports;
using Hodler.Domain.Users.Models;

namespace Hodler.Application.Portfolios.Services;

internal class PortfolioQueryService : IPortfolioQueryService
{
    private readonly ICurrentBitcoinPriceProvider _currentBitcoinPriceProvider;
    private readonly IHistoricalBitcoinPriceProvider _historicalBitcoinPriceProvider;
    private readonly IPortfolioRepository _portfolioRepository;

    public PortfolioQueryService(
        IPortfolioRepository portfolioRepository,
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider
    )
    {
        _portfolioRepository = portfolioRepository;
        _currentBitcoinPriceProvider = currentBitcoinPriceProvider;
        _historicalBitcoinPriceProvider = historicalBitcoinPriceProvider;
    }


    public async Task<PortfolioSummaryInfo> GetPortfolioSummaryAsync(UserId userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var portfolio = await FindOrCreatePortfolioAsync(userId, cancellationToken);

        var summary = await portfolio.GetSummaryReportAsync(_currentBitcoinPriceProvider, cancellationToken)!;

        return summary;
    }

    public async Task<IReadOnlyCollection<ChartCandle>> CalculatePortfolioValueChartAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        var portfolio = await _portfolioRepository.FindByAsync(userId, cancellationToken);

        if (portfolio is null)
        {
            return Array.Empty<ChartCandle>();
        }

        var chart = await portfolio
            .CalculatePortfolioValueChartAsync(_historicalBitcoinPriceProvider, cancellationToken);

        return chart;
    }

    public async Task<IPortfolio> FindOrCreatePortfolioAsync(
        UserId userId,
        CancellationToken cancellationToken)
    {
        var portfolio = await _portfolioRepository.FindByAsync(userId, cancellationToken);

        if (portfolio is null)
        {
            portfolio = Portfolio.CreateNew(userId);
            await _portfolioRepository.StoreAsync(portfolio, cancellationToken);
        }

        return portfolio;
    }
}