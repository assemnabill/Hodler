using Hodler.Domain.Portfolio.Models;
using MediatR;

namespace Hodler.Application.Portfolio.GetPortfolio;

public class GetPortfolioQuery(Guid userId) : IRequest<IPortfolio>
{
    public Guid UserId { get; } = userId;
}