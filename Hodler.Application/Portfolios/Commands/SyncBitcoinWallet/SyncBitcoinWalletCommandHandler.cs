using Hodler.Domain.Portfolios.Failures;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Commands.SyncBitcoinWallet;

public class SyncBitcoinWalletCommandHandler(IServiceScopeFactory serviceScopeFactory)
    : IRequestHandler<SyncBitcoinWalletCommand, IResult>
{
    public async Task<IResult> Handle(SyncBitcoinWalletCommand command, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var blockchainService = scope.ServiceProvider.GetRequiredService<IBitcoinBlockchainService>();
        var portfolioRepository = scope.ServiceProvider.GetRequiredService<IPortfolioRepository>();

        var portfolio = await portfolioRepository.FindByAsync(command.WalletId, cancellationToken);

        if (portfolio == null)
            return new FailureResult(new BitcoinWalletIsNotConnectedFailure(command.WalletId));

        var result = await portfolio.SyncBitcoinWalletAsync(
            command.WalletId,
            blockchainService,
            cancellationToken
        );

        if (result.Changed)
            await portfolioRepository.StoreAsync(portfolio, cancellationToken);

        return new SuccessResult();
    }
}