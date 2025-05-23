using Hodler.Domain.Portfolios.Models.BitcoinWallets;
using Hodler.Domain.Portfolios.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Queries.ConnectedWallets;

public class ConnectedWalletsQueryHandler(IServiceScopeFactory serviceScopeFactory)
    : IRequestHandler<ConnectedWalletsQuery, IReadOnlyCollection<IBitcoinWallet>>
{
    public async Task<IReadOnlyCollection<IBitcoinWallet>> Handle(
        ConnectedWalletsQuery query,
        CancellationToken cancellationToken
    )
    {
        using var scope = serviceScopeFactory.CreateScope();

        var portfolioQueryService = scope.ServiceProvider.GetRequiredService<IPortfolioQueryService>();

        var wallets = await portfolioQueryService
            .RetrieveBitcoinWalletsAsync(query.UserId, cancellationToken);

        return wallets;
    }
}