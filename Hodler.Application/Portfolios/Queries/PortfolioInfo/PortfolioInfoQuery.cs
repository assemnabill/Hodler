using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Queries.PortfolioInfo;

public class PortfolioInfoQuery(UserId userId) : IRequest<Domain.Portfolios.Models.PortfolioInfo>
{
    public UserId UserId { get; } = userId;
}