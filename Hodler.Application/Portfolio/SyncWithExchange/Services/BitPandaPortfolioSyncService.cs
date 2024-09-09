using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.BitPandaApi;
using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.Portfolio.Services.Sync;

namespace Hodler.Application.Portfolio.SyncWithExchange.Services;

public class BitPandaPortfolioSyncService : IPortfolioSyncService
{
    private readonly IBitPandaApiClient _bitPandaApiClient;
    private readonly IPortfolioQueryService _portfolioQueryService;
    private readonly IPortfolioRepository _portfolioRepository;

    public BitPandaPortfolioSyncService(
        IBitPandaApiClient bitPandaApiClient,
        IPortfolioQueryService portfolioQueryService, 
        IPortfolioRepository portfolioRepository)
    {
        _bitPandaApiClient = bitPandaApiClient;
        _portfolioQueryService = portfolioQueryService;
        _portfolioRepository = portfolioRepository;
    }

    public async Task<IPortfolio> SyncWithExchangeAsync(Guid userId, CancellationToken cancellationToken)
    {
        var transactions = await _bitPandaApiClient.GetTransactionsAsync(userId, cancellationToken);

        var portfolio = await _portfolioQueryService.GetByUserIdAsync(userId);
        portfolio = portfolio.SyncTransactions(transactions);

        await _portfolioRepository.StoreAsync(portfolio, cancellationToken);

        return portfolio;
    }
}