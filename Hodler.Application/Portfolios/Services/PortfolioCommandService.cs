using Corz.DomainDriven.Abstractions.Models.Results;
using Hodler.Application.Portfolios.Commands.AddTransaction;
using Hodler.Application.Portfolios.Commands.ModifyTransaction;
using Hodler.Application.Portfolios.Commands.RemoveTransaction;
using Hodler.Domain.BitcoinPrices.Ports;
using Hodler.Domain.Portfolios.Failures;
using Hodler.Domain.Portfolios.Ports.Repositories;
using Hodler.Domain.Portfolios.Services;

namespace Hodler.Application.Portfolios.Services;

public class PortfolioCommandService : IPortfolioCommandService
{
    private readonly IHistoricalBitcoinPriceProvider _historicalBitcoinPriceProvider;
    private readonly IPortfolioQueryService _portfolioQueryService;
    private readonly IPortfolioRepository _portfolioRepository;

    public PortfolioCommandService(
        IPortfolioQueryService portfolioQueryService,
        IPortfolioRepository portfolioRepository,
        IHistoricalBitcoinPriceProvider historicalBitcoinPriceProvider
    )
    {
        _portfolioQueryService = portfolioQueryService;
        _portfolioRepository = portfolioRepository;
        _historicalBitcoinPriceProvider = historicalBitcoinPriceProvider;
    }

    public async Task<IResult> AddTransactionAsync(
        AddTransactionCommand request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        var portfolio = await _portfolioQueryService.FindOrCreatePortfolioAsync(request.UserId, cancellationToken);

        var result = await portfolio.AddTransactionAsync(
            _historicalBitcoinPriceProvider,
            request.Type,
            request.Date,
            request.Price,
            request.Amount,
            request.TransactionSource,
            cancellationToken
        );

        if (result.IsSuccess)
            await _portfolioRepository.StoreAsync(portfolio, cancellationToken);

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

    public async Task<IResult> ModifyTransactionAsync(ModifyTransactionCommand request, CancellationToken cancellationToken = default)
    {
        var portfolio = await _portfolioQueryService
            .FindPortfolioAsync(request.UserId, cancellationToken);

        if (portfolio is null)
            return new FailureResult(new NoPortfolioFoundForUserFailure(request.UserId));

        var result = await portfolio.ModifyTransactionAsync(
            _historicalBitcoinPriceProvider,
            request.TransactionId,
            request.Type,
            request.Date,
            request.Price,
            request.Amount,
            request.TransactionSource,
            cancellationToken
        );

        if (result.IsSuccess)
            await _portfolioRepository.StoreAsync(portfolio, cancellationToken);

        return !result.IsSuccess && result.Failures.First() is TransactionAlreadyExistsFailure
            ? new SuccessResult()
            : result;
    }
}