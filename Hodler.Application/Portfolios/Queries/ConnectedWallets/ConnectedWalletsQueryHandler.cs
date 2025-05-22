using Hodler.Domain.Portfolios.Services;
using Mapster;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Queries.ConnectedWallets;

public class ConnectedWalletsQueryHandler(IServiceScopeFactory serviceScopeFactory)
    : IRequestHandler<ConnectedWalletsQuery, IReadOnlyCollection<BitcoinWalletDto>>
{
    public async Task<IReadOnlyCollection<BitcoinWalletDto>> Handle(
        ConnectedWalletsQuery query,
        CancellationToken cancellationToken
    )
    {
        using var scope = serviceScopeFactory.CreateScope();

        var portfolioQueryService = scope.ServiceProvider.GetRequiredService<IPortfolioQueryService>();

        var wallets = await portfolioQueryService
            .RetrieveBitcoinWalletsAsync(query.UserId, cancellationToken);

        return wallets.Adapt<IReadOnlyCollection<BitcoinWalletDto>>();
    }
}