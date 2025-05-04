using Hodler.Application.Portfolios.Commands.AddTransaction;
using Hodler.Application.Portfolios.Commands.RemoveTransaction;
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

namespace Hodler.ApiService.Portfolios;

public class PortfolioController(IMediator mediator) : ApiController
{
    [HttpGet("transactions")]
    [ProducesResponseType(typeof(PortfolioTransactionsDto), 200)]
    public async Task<IActionResult> GetPortfolioTransactionsAsync(CancellationToken cancellationToken)
    {
        var request = new PortfolioInfoQuery(UserId);
        var response = await mediator.Send(request, cancellationToken);
        var dto = response.Adapt<PortfolioTransactionsDto>();

        return Ok(dto);
    }

    [HttpGet("valueChart")]
    [ProducesResponseType(typeof(PortfolioValueChartDto), 200)]
    public async Task<IActionResult> GetPortfolioValueChartAsync(CancellationToken cancellationToken)
    {
        var request = new PortfolioValueChartQuery(UserId);
        var response = await mediator.Send(request, cancellationToken);
        var dto = response.Adapt<PortfolioValueChartDto>();

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
            addTransactionRequestContract.Type,
            (CryptoExchangeName?)addTransactionRequestContract.CryptoExchange
        );

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result.Failures);
    }

    [HttpDelete("transaction/{transactionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveTransactionAsync(
        [FromRoute] Guid transactionId,
        CancellationToken cancellationToken
    )
    {
        var request = new RemoveTransactionCommand(
            UserId,
            new TransactionId(transactionId)
        );

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result.Failures);
    }

    [HttpPost("sync/{exchangeNamesName}")]
    [ProducesResponseType(typeof(PortfolioTransactionsDto), 200)]
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
        var dto = result.Adapt<PortfolioTransactionsDto>();

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