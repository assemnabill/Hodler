using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Queries.PortfolioSummary;

public class PortfolioSummaryQuery : IRequest<Domain.Portfolios.Models.PortfolioSummaryInfo>
{
    public UserId UserId { get; }

    public PortfolioSummaryQuery(UserId userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
}