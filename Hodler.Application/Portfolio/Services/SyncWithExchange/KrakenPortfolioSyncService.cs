using Hodler.Domain.CryptoExchange.Ports.CryptoExchangeApis;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.User.Models;
using Mapster;

namespace Hodler.Application.Portfolio.Services.SyncWithExchange;

public class KrakenPortfolioSyncService : IPortfolioSyncService
{
    private readonly IKrakenApiClient _krakenApiClient;
    private readonly IPortfolioQueryService _portfolioQueryService;
    private readonly IPortfolioRepository _portfolioRepository;

    public KrakenPortfolioSyncService(
        IKrakenApiClient krakenApiClient,
        IPortfolioQueryService portfolioQueryService,
        IPortfolioRepository portfolioRepository)
    {
        _krakenApiClient = krakenApiClient;
        _portfolioQueryService = portfolioQueryService;
        _portfolioRepository = portfolioRepository;
    }

    public async Task<IPortfolio> SyncWithExchangeAsync(
        UserId userId,
        CancellationToken cancellationToken
    )
    {
        var transactionInfos = await _krakenApiClient.GetTransactionsAsync(userId, cancellationToken);

        var portfolio = await _portfolioQueryService.GetByUserIdAsync(userId, cancellationToken);

        var transactions = transactionInfos
            .Select(info => (portfolio.Id, info).Adapt<Transaction>());

        var syncResult = portfolio.SyncTransactions(transactions);

        if (syncResult.Changed)
        {
            await _portfolioRepository.StoreAsync(portfolio, cancellationToken);
        }

        return syncResult.CurrentState;
    }
}