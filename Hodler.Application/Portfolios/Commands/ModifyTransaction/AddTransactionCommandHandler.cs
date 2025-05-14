using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Application.Portfolios.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Commands.ModifyTransaction;

public class ModifyTransactionCommandHandler : IRequestHandler<ModifyTransactionCommand, IResult>
{
    private readonly IServiceProvider _serviceProvider;

    public ModifyTransactionCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IResult> Handle(
        ModifyTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var domainService = _serviceProvider.GetRequiredService<IPortfolioCommandService>();

        var result = await domainService.ModifyTransactionAsync(request, cancellationToken);

        return result;
    }
}