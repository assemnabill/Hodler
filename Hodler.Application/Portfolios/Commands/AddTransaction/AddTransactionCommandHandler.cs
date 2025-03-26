using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Application.Portfolios.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Commands.AddTransaction;

public class AddTransactionCommandHandler : IRequestHandler<AddTransactionCommand, IResult>
{
    private readonly IServiceProvider _serviceProvider;

    public AddTransactionCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<IResult> Handle(
        AddTransactionCommand request,
        CancellationToken cancellationToken
    )
    {
        var domainService = _serviceProvider.GetRequiredService<IPortfolioCommandService>();

        var result = await domainService.AddTransactionAsync(request, cancellationToken);

        return result;
    }
}