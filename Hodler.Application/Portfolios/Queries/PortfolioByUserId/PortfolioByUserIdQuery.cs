using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Queries.PortfolioByUserId;

public class PortfolioByUserIdQuery(UserId userId) : IRequest<PortfolioInfo>
{
    public UserId UserId { get; } = userId;
}