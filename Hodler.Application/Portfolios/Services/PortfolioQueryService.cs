using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Services;
using Microsoft.Extensions.Internal;

namespace Hodler.Application.Portfolios.Services;

internal class PortfolioQueryService : IPortfolioQueryService
{
    private readonly ICurrentBitcoinPriceProvider _currentBitcoinPriceProvider;
    private readonly IHistoricalBitcoinPriceProvider _historicalBitcoinPriceProvider;
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly ISystemClock _systemClock;
    private readonly IUserSettingsService _userSettingsService;

    public PortfolioQueryService(
        IPortfolioRepository portfolioRepository,
        ICurrentBitcoinPriceProvider currentBitcoinPriceProvider,
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider,
        IUserSettingsService userSettingsService,
        ISystemClock systemClock
    )
    {
        _portfolioRepository = portfolioRepository;
        _currentBitcoinPriceProvider = currentBitcoinPriceProvider;
        _historicalBitcoinPriceProvider = historicalBitcoinPriceProvider;
        _userSettingsService = userSettingsService;
        _systemClock = systemClock;
    }


    public async Task<PortfolioSummaryInfo> GetPortfolioSummaryAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var portfolio = await FindOrCreatePortfolioAsync(userId, cancellationToken);
        var userSettings = await _userSettingsService.GetUserSettingsAsync(userId, cancellationToken);

        var summary = await portfolio.GetSummaryReportAsync(_currentBitcoinPriceProvider, userSettings.DisplayCurrency, cancellationToken);

        return summary;
    }

    public async Task<PortfolioValueChartInfo> CalculatePortfolioValueChartAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        var portfolio = await FindOrCreatePortfolioAsync(userId, cancellationToken);
        var userSettings = await _userSettingsService.GetUserSettingsAsync(userId, cancellationToken);

        var chart = await portfolio
            .CalculatePortfolioValueChartAsync(
                _historicalBitcoinPriceProvider,
                userSettings.DisplayCurrency,
                _systemClock,
                cancellationToken
            );

        var portfolioValue = await portfolio
            .GetSummaryReportAsync(
                _currentBitcoinPriceProvider,
                userSettings.DisplayCurrency,
                cancellationToken
            );

        var chartInfo = new PortfolioValueChartInfo(
            chart,
            portfolioValue.PortfolioValue,
            portfolioValue.FiatReturnOnInvestmentPercentage
        );

        return chartInfo;
    }

    public async Task<IPortfolio> FindOrCreatePortfolioAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        var portfolio = await FindPortfolioAsync(userId, cancellationToken);

        if (portfolio is not null)
            return portfolio;

        portfolio = Portfolio.CreateNew(userId);
        await _portfolioRepository.StoreAsync(portfolio, cancellationToken);

        return portfolio;
    }

    public async Task<IPortfolio?> FindPortfolioAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        var portfolio = await _portfolioRepository.FindByAsync(userId, cancellationToken);

        return portfolio;
    }

    public async Task<IReadOnlyCollection<IBitcoinWallet>> RetrieveBitcoinWalletsAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        var portfolio = await _portfolioRepository.FindByAsync(userId, cancellationToken);
        return portfolio?.BitcoinWallets ?? new BitcoinWallets([]);
    }
}