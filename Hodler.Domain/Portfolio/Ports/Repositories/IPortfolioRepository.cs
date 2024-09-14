using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Ports.Repositories;

public interface IPortfolioRepository : IRepository<IPortfolio>
{
    Task<IPortfolio?> FindByAsync(UserId userId, CancellationToken cancellationToken);
}