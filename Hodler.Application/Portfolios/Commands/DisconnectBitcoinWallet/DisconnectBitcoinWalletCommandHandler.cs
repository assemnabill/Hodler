using Hodler.Domain.Portfolios.Failures;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Shared.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Hodler.Application.Portfolios.Commands.DisconnectBitcoinWallet;

public class DisconnectBitcoinWalletCommandHandler(IServiceScopeFactory serviceScopeFactory)
    : IRequestHandler<DisconnectBitcoinWalletCommand, IResult>
{
    public async Task<IResult> Handle(DisconnectBitcoinWalletCommand command, CancellationToken cancellationToken)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var portfolioRepository = scope.ServiceProvider.GetRequiredService<IPortfolioRepository>();

        var portfolio = await portfolioRepository.FindByAsync(command.WalletId, cancellationToken);

        if (portfolio == null)
            return new FailureResult(new BitcoinWalletIsNotConnectedFailure(command.WalletId));

        var result = portfolio.DisconnectBitcoinWallet(command.WalletId);

        if (result.IsSuccess)
            await portfolioRepository.StoreAsync(portfolio, cancellationToken);

        return result;
    }
}