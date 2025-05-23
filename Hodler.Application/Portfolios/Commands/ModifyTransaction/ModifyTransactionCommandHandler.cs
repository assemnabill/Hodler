using Hodler.Application.Portfolios.Services;
using Hodler.Domain.Shared.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Commands.ModifyTransaction;

public class ModifyTransactionCommandHandler(IServiceScopeFactory serviceScopeFactory) : IRequestHandler<ModifyTransactionCommand, IResult>
{
    public async Task<IResult> Handle(
        ModifyTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var serviceProvider = scope.ServiceProvider;

        var domainService = serviceProvider.GetRequiredService<IPortfolioCommandService>();
        var result = await domainService.ModifyTransactionAsync(request, cancellationToken);

        return result;
    }
}