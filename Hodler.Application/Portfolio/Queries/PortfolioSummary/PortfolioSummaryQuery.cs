using Hodler.Domain.User.Models;
using MediatR;

namespace Hodler.Application.Portfolio.Queries.PortfolioSummary;

public class PortfolioSummaryQuery : IRequest<Domain.Portfolio.Models.PortfolioSummary>
{
    public UserId UserId { get; }

    public PortfolioSummaryQuery(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}