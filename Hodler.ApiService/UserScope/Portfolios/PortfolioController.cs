using Hodler.Application.Portfolios.Commands.AddTransaction;
using Hodler.Application.Portfolios.Commands.SyncWithExchange;
using Hodler.Application.Portfolios.Queries.PortfolioInfo;
using Hodler.Application.Portfolios.Queries.PortfolioSummary;
using Hodler.Application.Portfolios.Queries.PortfolioValueChart;
using Hodler.Contracts.Portfolios.AddTransaction;
using Hodler.Contracts.Portfolios.PortfolioSummary;
using Hodler.Contracts.Portfolios.PortfolioValueChart;
using Hodler.Contracts.Shared;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models;
using Hodler.Domain.Shared.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.UserScope.Portfolios;

public class PortfolioController(IMediator mediator) : ApiController
{
    [HttpGet]
    [ProducesResponseType(typeof(PortfolioInfoDto), 200)]
    public async Task<IActionResult> GetPortfolioAsync(CancellationToken cancellationToken)
    {
        var request = new PortfolioInfoQuery(UserId);
        var response = await mediator.Send(request, cancellationToken);
        var dto = response.Adapt<PortfolioInfoDto>();

        return Ok(dto);
    }

    [HttpGet("valueChart")]
    [ProducesResponseType(typeof(IReadOnlyCollection<ChartCandleDto>), 200)]
    public async Task<IActionResult> GetPortfolioValueChartAsync(CancellationToken cancellationToken)
    {
        var request = new PortfolioValueChartQuery(UserId);
        var response = await mediator.Send(request, cancellationToken);
        var dto = response.Adapt<IReadOnlyCollection<ChartCandleDto>>();

        return Ok(dto);
    }

    [HttpPost("transaction")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddTransactionAsync(
        [FromBody] AddTransactionRequestContract addTransactionRequestContract,
        CancellationToken cancellationToken
    )
    {
        var request = new AddTransactionCommand(
            UserId,
            addTransactionRequestContract.Date,
            addTransactionRequestContract.BitcoinAmount,
            addTransactionRequestContract.FiatAmount.Adapt<FiatAmount>(),
            (TransactionType)addTransactionRequestContract.Type,
            (CryptoExchangeName)addTransactionRequestContract.CryptoExchange
        );

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result.Failures);
    }

    [HttpPost("sync/{exchangeNamesName}")]
    [ProducesResponseType(typeof(PortfolioInfoDto), 200)]
    public async Task<IActionResult> SyncWithExchangeAsync(
        CryptoExchangeNames exchangeNamesName,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(exchangeNamesName);

        var request = new SyncWithExchangeCommand(
            UserId,
            (CryptoExchangeName)exchangeNamesName
        );

        var result = await mediator.Send(request, cancellationToken);
        var dto = result.Adapt<PortfolioInfoDto>();

        return Ok(dto);
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(PortfolioSummaryDto), 200)]
    public async Task<IActionResult> GetPortfolioSummaryAsync(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PortfolioSummaryQuery(UserId), cancellationToken);

        var dto = result.Adapt<PortfolioSummaryDto>();

        return Ok(dto);
    }
}