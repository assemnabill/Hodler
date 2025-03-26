using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Portfolios.Ports.Repositories;

public interface IPortfolioRepository : IRepository<IPortfolio>
{
    Task<IPortfolio?> FindByAsync(UserId userId, CancellationToken cancellationToken);
}