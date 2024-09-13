using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.Portfolio.Services;

public interface IPortfolioQueryService   
{
    Task<IPortfolio> GetByUserIdAsync(UserId userId);
}