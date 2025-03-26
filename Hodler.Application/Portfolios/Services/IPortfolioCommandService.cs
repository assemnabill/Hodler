using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Application.Portfolios.Commands.AddTransaction;

namespace Hodler.Application.Portfolios.Services;

public interface IPortfolioCommandService
{
    Task<IResult> AddTransactionAsync(AddTransactionCommand request, CancellationToken cancellationToken = default);
}