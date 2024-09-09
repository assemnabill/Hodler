using Hodler.Domain.Portfolio.Models;

namespace Hodler.Domain.Portfolio.Services;

public interface IPortfolioQueryService   
{
    Task<IPortfolio> GetByUserIdAsync(Guid userId);
}