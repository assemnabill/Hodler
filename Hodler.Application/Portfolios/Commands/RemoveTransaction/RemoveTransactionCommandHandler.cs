using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Application.Portfolios.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Commands.RemoveTransaction;

public class RemoveTransactionCommandHandler : IRequestHandler<RemoveTransactionCommand, IResult>
{
    private readonly IServiceProvider _serviceProvider;

    public RemoveTransactionCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IResult> Handle(
        RemoveTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var domainService = _serviceProvider.GetRequiredService<IPortfolioCommandService>();

        var result = await domainService.RemoveTransactionAsync(request, cancellationToken);

        return result;
    }
}