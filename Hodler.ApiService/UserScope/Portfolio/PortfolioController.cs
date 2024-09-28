using Hodler.Application.Portfolio.Commands.AddTransaction;
using Hodler.Application.Portfolio.Commands.SyncWithExchange;
using Hodler.Application.Portfolio.Queries.PortfolioByUserId;
using Hodler.Application.Portfolio.Queries.PortfolioSummary;
using Hodler.Contracts.Portfolio.PortfolioSummary;
using Hodler.Contracts.Shared;
using Hodler.Domain.User.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.UserScope.Portfolio;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly IMediator _mediator;

    public PortfolioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // TODO: Add authentication
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPortfolioByUserId(string userId, CancellationToken cancellationToken)
    {
        var request = new PortfolioByUserIdQuery(new UserId(Guid.Parse(userId)));
        var response = await _mediator.Send(request, cancellationToken);
        var dto = response.Adapt<PortfolioInfoDto>();

        return Ok(dto);
    }

    // TODO: Add authentication and implementation
    [HttpPost("transaction")]
    public async Task<IActionResult> AddTransaction(
        [FromBody] AddTransactionRequestContract addTransactionRequestContract,
        CancellationToken cancellationToken)
    {
        var request = new AddTransactionCommand(
            addTransactionRequestContract.UserId,
            addTransactionRequestContract.Date,
            addTransactionRequestContract.Amount,
            addTransactionRequestContract.Price,
            addTransactionRequestContract.Type
        );

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }

    // TODO: Add authentication
    [HttpPost("sync/{exchangeNamesName}")]
    public async Task<IActionResult> SyncWithExchange(CryptoExchangeNames exchangeNamesName,
        [FromBody] string userId,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(exchangeNamesName);
        ArgumentNullException.ThrowIfNull(userId);
        
        var request = new SyncWithExchangeCommand(
            new UserId(Guid.Parse(userId)),
            (Domain.CryptoExchange.Models.CryptoExchangeNames)exchangeNamesName
        );

        var result = await _mediator.Send(request, cancellationToken);
        var dto = result.Adapt<PortfolioInfoDto>();

        return Ok(dto);
    }

    [HttpGet("{userId}/summary")]
    public async Task<IActionResult> GetPortfolioSummary(
        string userId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new PortfolioSummaryQuery(new UserId(Guid.Parse(userId))), cancellationToken);

        var dto = result.Adapt<PortfolioSummaryDto>();

        return Ok(dto);
    }
}