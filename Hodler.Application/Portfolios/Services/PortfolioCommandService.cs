using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Application.Portfolios.Commands.AddTransaction;
using Hodler.Application.Portfolios.Commands.RemoveTransaction;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Portfolios.Services;

namespace Hodler.Application.Portfolios.Services;

public class PortfolioCommandService : IPortfolioCommandService
{
    private readonly IPortfolioQueryService _portfolioQueryService;
    private readonly IPortfolioRepository _portfolioRepository;

    public PortfolioCommandService(
        IPortfolioQueryService portfolioQueryService,
        IPortfolioRepository portfolioRepository
    )
    {
        _portfolioQueryService = portfolioQueryService;
        _portfolioRepository = portfolioRepository;
    }

    public async Task<IResult> AddTransactionAsync(
        AddTransactionCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var portfolio = await _portfolioQueryService.FindOrCreatePortfolioAsync(request.UserId, cancellationToken);

        var result = portfolio.AddTransaction(
            request.Type,
            request.Date,
            request.Price,
            request.Amount,
            request.CryptoExchange
        );

        if (result.IsSuccess)
        {
            await _portfolioRepository.StoreAsync(portfolio, cancellationToken);
        }

        return result;
    }

    public async Task<IResult> RemoveTransactionAsync(RemoveTransactionCommand request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var portfolio = await _portfolioQueryService.FindOrCreatePortfolioAsync(request.UserId, cancellationToken);
        var result = portfolio.RemoveTransaction(request.TransactionId);

        if (result.IsSuccess)
            await _portfolioRepository.StoreAsync(portfolio, cancellationToken);

        return result;
    }
}