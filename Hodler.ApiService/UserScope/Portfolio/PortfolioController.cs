using Hodler.Application.Portfolio.AddTransaction;
using Hodler.Application.Portfolio.GetPortfolio;
using Hodler.Application.Portfolio.SyncWithExchange;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.User.Models;
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

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPortfolio(Guid userId)
    {
        var portfolio = await _mediator.Send(new GetPortfolioQuery(userId));
        return Ok(portfolio);
    }

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

    [HttpPost("sync/{exchangeName}")]
    public async Task<IActionResult> SyncWithExchange([FromBody] Guid userId, [FromQuery] CryptoExchange exchangeName)
    {
        var result = await _mediator.Send(new SyncWithExchangeCommand(new UserId(userId), exchangeName));

        return Ok(result);
    }
}