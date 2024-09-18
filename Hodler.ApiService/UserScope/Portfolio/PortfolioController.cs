using Hodler.Application.Portfolio.Commands.AddTransaction;
using Hodler.Application.Portfolio.Commands.SyncWithExchange;
using Hodler.Application.Portfolio.Queries.PortfolioByUserId;
using Hodler.Application.Portfolio.Queries.PortfolioSummary;
using Hodler.Contracts.Portfolio.PortfolioSummary;
using Hodler.Domain.Shared.Models;
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
    public async Task<IActionResult> GetPortfolioByUserId(Guid userId)
    {
        var portfolio = await _mediator.Send(new PortfolioByUserIdQuery(new UserId(userId)));
        var dto = portfolio.Adapt<PortfolioInfoDto>();

        return Ok(dto);
    }

    // TODO: Add authentication and implementation
    [HttpPost("transaction")]
    public async Task<IActionResult> AddTransaction(
        [FromBody] AddTransactionRequestContract addTransactionRequestContract)
    {
        await _mediator.Send(new AddTransactionCommand(
            addTransactionRequestContract.UserId,
            addTransactionRequestContract.Date,
            addTransactionRequestContract.Amount,
            addTransactionRequestContract.Price,
            addTransactionRequestContract.Type
        ));

        return Ok();
    }

    // TODO: Add authentication
    [HttpPost("sync/{exchangeName}")]
    public async Task<IActionResult> SyncWithExchange(
        [FromBody] Guid userId,
        [FromQuery] CryptoExchange exchangeName,
        CancellationToken cancellationToken
    )
    {
        var result = await _mediator.Send(new SyncWithExchangeCommand(new UserId(userId), exchangeName), cancellationToken);

        return Ok(result);
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