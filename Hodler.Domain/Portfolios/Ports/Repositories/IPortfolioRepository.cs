using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Shared.Aggregate;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Ports.Repositories;

public interface IPortfolioRepository : IRepository<IPortfolio>
{
    Task<IPortfolio?> FindByAsync(UserId userId, CancellationToken cancellationToken = default);
    Task<IPortfolio?> FindByAsync(PortfolioId portfolioId, CancellationToken cancellationToken = default);
    Task<IPortfolio?> FindByAsync(BitcoinWalletId walletId, CancellationToken cancellationToken = default);
}