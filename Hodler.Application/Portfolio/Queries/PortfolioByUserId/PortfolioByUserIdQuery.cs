using Hodler.Domain.Portfolio.Models;
using Hodler.Domain.User.Models;
using MediatR;

namespace Hodler.Application.Portfolio.Queries.PortfolioByUserId;

public class PortfolioByUserIdQuery(UserId userId) : IRequest<PortfolioInfo>
{
    public UserId UserId { get; } = userId;
}