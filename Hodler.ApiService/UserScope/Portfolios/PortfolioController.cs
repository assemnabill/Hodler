using Hodler.Application.Portfolios.Commands.AddTransaction;
using Hodler.Application.Portfolios.Commands.SyncWithExchange;
using Hodler.Application.Portfolios.Queries.PortfolioByUserId;
using Hodler.Application.Portfolios.Queries.PortfolioSummary;
using Hodler.Contracts.Portfolios.AddTransaction;
using Hodler.Contracts.Portfolios.PortfolioSummary;
using Hodler.Contracts.Shared;
using Hodler.Domain.Portfolios.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.UserScope.Portfolios;

public class PortfolioController : ApiController
{
    private readonly IMediator _mediator;

    public PortfolioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PortfolioInfoDto), 200)]
    public async Task<IActionResult> GetPortfolioByUserId(CancellationToken cancellationToken)
    {
        var request = new PortfolioByUserIdQuery(UserId);
        var response = await _mediator.Send(request, cancellationToken);
        var dto = response.Adapt<PortfolioInfoDto>();

        return Ok(dto);
    }

    [HttpPost("transaction")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> AddTransaction(
        [FromBody] AddTransactionRequestContract addTransactionRequestContract,
        CancellationToken cancellationToken)
    {
        var request = new AddTransactionCommand(
            UserId,
            addTransactionRequestContract.Date,
            addTransactionRequestContract.Amount,
            addTransactionRequestContract.Price,
            (TransactionType)addTransactionRequestContract.Type
        );

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    [HttpPost("sync/{exchangeNamesName}")]
    [ProducesResponseType(typeof(PortfolioInfoDto), 200)]
    public async Task<IActionResult> SyncWithExchange(
        CryptoExchangeNames exchangeNamesName,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(exchangeNamesName);

        var request = new SyncWithExchangeCommand(
            UserId,
            (Domain.CryptoExchanges.Models.CryptoExchangeNames)exchangeNamesName
        );

        var result = await _mediator.Send(request, cancellationToken);
        var dto = result.Adapt<PortfolioInfoDto>();

        return Ok(dto);
    }

    [HttpGet("summary")]
    [ProducesResponseType(typeof(PortfolioSummaryDto), 200)]
    public async Task<IActionResult> GetPortfolioSummary(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new PortfolioSummaryQuery(UserId), cancellationToken);

        var dto = result.Adapt<PortfolioSummaryDto>();

        return Ok(dto);
    }
}