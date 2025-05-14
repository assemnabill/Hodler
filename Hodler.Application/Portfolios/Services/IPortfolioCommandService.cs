using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Application.Portfolios.Commands.AddTransaction;
using Hodler.Application.Portfolios.Commands.ModifyTransaction;
using Hodler.Application.Portfolios.Commands.RemoveTransaction;

namespace Hodler.Application.Portfolios.Services;

public interface IPortfolioCommandService
{
    Task<IResult> AddTransactionAsync(AddTransactionCommand request, CancellationToken cancellationToken = default);
    Task<IResult> RemoveTransactionAsync(RemoveTransactionCommand request, CancellationToken cancellationToken = default);
    Task<IResult> ModifyTransactionAsync(ModifyTransactionCommand request, CancellationToken cancellationToken = default);
}