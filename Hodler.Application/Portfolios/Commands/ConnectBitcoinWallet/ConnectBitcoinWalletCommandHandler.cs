using Hodler.Domain.Portfolios.Failures;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Portfolios.Services;
using Hodler.Domain.Shared.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Commands.ConnectBitcoinWallet;

public class ConnectBitcoinWalletCommandHandler(IServiceScopeFactory serviceScopeFactory) : IRequestHandler<ConnectBitcoinWalletCommand, IResult>
{
    public async Task<IResult> Handle(ConnectBitcoinWalletCommand command, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var portfolioRepository = scope.ServiceProvider.GetRequiredService<IPortfolioRepository>();

        var portfolio = await portfolioRepository.FindByAsync(command.UserId, cancellationToken);

        if (portfolio == null)
            return new FailureResult(new NoPortfolioFoundForUserFailure(command.UserId));

        var blockchainService = scope.ServiceProvider.GetRequiredService<IBitcoinBlockchainService>();

        var result = await portfolio.ConnectBitcoinWallet(
            command.Address,
            command.WalletName,
            blockchainService,
            command.Avatar,
            cancellationToken
        );

        if (result.IsSuccess)
            await portfolioRepository.StoreAsync(portfolio, cancellationToken);

        return result;
    }
}