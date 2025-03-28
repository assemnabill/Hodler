using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Users.Models;
using MediatR;

namespace Hodler.Application.Portfolios.Queries.PortfolioValueChart;

public class PortfolioValueChartQuery(UserId userId) : IRequest<IReadOnlyCollection<ChartSpot>>
{
    public UserId UserId { get; } = userId;
}