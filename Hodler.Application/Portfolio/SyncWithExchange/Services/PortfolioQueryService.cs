using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.Portfolio.Ports.Repositories;
using Hodler.Domain.Portfolio.Services;
using Hodler.Domain.User.Models;

namespace Hodler.Application.Portfolio.SyncWithExchange.Services;

internal class PortfolioQueryService : IPortfolioQueryService
{
    private readonly IPortfolioRepository _portfolioRepository;

    public PortfolioQueryService(
        IPortfolioRepository portfolioRepository
    )
    {
        _portfolioRepository = portfolioRepository;
    }

    public async Task<IPortfolio?> GetByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        return await _portfolioRepository.FindByAsync(userId, cancellationToken);
    }
}