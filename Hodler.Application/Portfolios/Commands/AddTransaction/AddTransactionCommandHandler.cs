using Hodler.Application.Portfolios.Services;
using Hodler.Domain.Shared.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Commands.AddTransaction;

public class AddTransactionCommandHandler(IServiceProvider serviceProvider)
    : IRequestHandler<AddTransactionCommand, IResult>
{
    public async Task<IResult> Handle(
        AddTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var domainService = serviceProvider.GetRequiredService<IPortfolioCommandService>();

        var result = await domainService.AddTransactionAsync(request, cancellationToken);

        return result;
    }
}