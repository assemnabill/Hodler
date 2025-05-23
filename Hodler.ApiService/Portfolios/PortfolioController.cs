using Hodler.Application.Portfolios.Commands.AddTransaction;
using Hodler.Application.Portfolios.Commands.ModifyTransaction;
using Hodler.Application.Portfolios.Commands.RemoveTransaction;
using Hodler.Application.Portfolios.Commands.SyncWithExchange;
using Hodler.Application.Portfolios.Queries.PortfolioInfo;
using Hodler.Application.Portfolios.Queries.PortfolioSummary;
using Hodler.Application.Portfolios.Queries.PortfolioValueChart;
using Hodler.Contracts.Portfolios.AddTransaction;
using Hodler.Contracts.Portfolios.ModifyTransaction;
using Hodler.Contracts.Portfolios.PortfolioSummary;
using Hodler.Contracts.Portfolios.PortfolioValueChart;
using Hodler.Contracts.Shared;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Portfolios.Models.Transactions;
using Hodler.Domain.Shared.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.Portfolios;

/// <summary>
/// Handles portfolio-related operations such as retrieving summaries, charts, and transactions,
/// as well as managing transactions and synchronizing data with external exchanges.
/// </summary>
public class PortfolioController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Retrieves a summarized view of the user's portfolio, including details such as net investment,
    /// total Bitcoin holdings, current Bitcoin price, portfolio value, return on investment, and
    /// other related metrics.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests, used to terminate the asynchronous operation early if necessary.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the portfolio summary data encapsulated in a <see cref="PortfolioSummaryDto"/> object.
    /// </returns>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(PortfolioSummaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPortfolioSummaryAsync(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new PortfolioSummaryQuery(UserId), cancellationToken);
        var dto = result.Adapt<PortfolioSummaryDto>();

        return Ok(dto);
    }

    /// <summary>
    /// Retrieves the portfolio value chart data for the authenticated user,
    /// providing details such as chart points representing portfolio value over time,
    /// total portfolio value in fiat currency, and return on investment percentage.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests, used to terminate the asynchronous operation early if necessary.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the portfolio value chart data encapsulated in a <see cref="PortfolioValueChartDto"/> object.
    /// </returns>
    [HttpGet("valueChart")]
    [ProducesResponseType(typeof(PortfolioValueChartDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPortfolioValueChartAsync(CancellationToken cancellationToken)
    {
        var request = new PortfolioValueChartQuery(UserId);
        var response = await mediator.Send(request, cancellationToken);
        var dto = response.Adapt<PortfolioValueChartDto>();

        return Ok(dto);
    }

    /// <summary>
    /// Retrieves a list of transactions associated with the user's portfolio, including transaction-specific
    /// details such as type, amount, market price, and timestamp.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests, used to terminate the asynchronous operation early if necessary.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the portfolio transactions data encapsulated in a <see cref="PortfolioTransactionsDto"/> object.
    /// </returns>
    [HttpGet("transactions")]
    [ProducesResponseType(typeof(PortfolioTransactionsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPortfolioTransactionsAsync(CancellationToken cancellationToken)
    {
        var request = new PortfolioInfoQuery(UserId);
        var response = await mediator.Send(request, cancellationToken);
        var dto = response.Adapt<PortfolioTransactionsDto>();

        return Ok(dto);
    }

    /// <summary>
    /// Adds a new transaction to the user's portfolio, including details such as date, Bitcoin amount, fiat amount,
    /// transaction type, and optionally the crypto exchange through which the transaction was executed.
    /// </summary>
    /// <param name="addTransactionRequestContract">
    /// The request containing the transaction details, including date, Bitcoin amount, fiat amount, transaction type,
    /// and the optional crypto exchange name.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests, used to terminate the asynchronous operation early if necessary.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the operation. A successful addition returns a StatusCodes.Status200OK OK
    /// response, while failures return a 400 Bad Request response.
    /// </returns>
    [HttpPost("transactions")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddTransactionAsync(
        [FromBody] AddTransactionRequestContract addTransactionRequestContract,
        CancellationToken cancellationToken
    )
    {
        var request = new AddTransactionCommand(
            UserId,
            addTransactionRequestContract.Timestamp,
            addTransactionRequestContract.BitcoinAmount,
            addTransactionRequestContract.FiatAmount.Adapt<FiatAmount>(),
            addTransactionRequestContract.Type,
            addTransactionRequestContract.TransactionSource.Adapt<ITransactionSource>()
        );

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result.Failures);
    }


    /// <summary>
    /// Modify a new transaction to the user's portfolio, including details such as date, Bitcoin amount, fiat amount,
    /// transaction type, and optionally the crypto exchange through which the transaction was executed.
    /// </summary>
    /// <param name="transactionId">the unique identifier of the transaction</param>
    /// <param name="modifyTransactionRequestContract">
    /// The request containing the transaction details, including date, Bitcoin amount, fiat amount, transaction type,
    /// and the optional crypto exchange name.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests, used to terminate the asynchronous operation early if necessary.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the outcome of the operation. A successful addition returns a StatusCodes.Status200OK OK
    /// response, while failures return a 400 Bad Request response.
    /// </returns>
    [HttpPost("transactions/{transactionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ModifyTransactionAsync(
        [FromRoute] Guid transactionId,
        [FromBody] ModifyTransactionRequestContract modifyTransactionRequestContract,
        CancellationToken cancellationToken
    )
    {
        var request = new ModifyTransactionCommand(
            new TransactionId(transactionId),
            UserId,
            modifyTransactionRequestContract.Timestamp,
            modifyTransactionRequestContract.BitcoinAmount,
            modifyTransactionRequestContract.FiatAmount.Adapt<FiatAmount>(),
            modifyTransactionRequestContract.Type,
            modifyTransactionRequestContract.TransactionSource.Adapt<ITransactionSource>()
        );

        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok(result) : BadRequest(result.Failures);
    }


    /// <summary>
    /// Removes a transaction from the user's portfolio based on the specified transaction ID.
    /// </summary>
    /// <param name="transactionId">
    /// The unique identifier of the transaction to be removed from the portfolio.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests, used to terminate the asynchronous operation early if necessary.
    /// </param>
    /// <returns>
    /// An <see cref="IActionResult"/> indicating the result of the operation. Returns No Content (204) on success
    /// or Bad Request (400) if the operation fails.
    /// </returns>
    [HttpDelete("transactions/{transactionId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveTransactionAsync(
        [FromRoute] Guid transactionId,
        CancellationToken cancellationToken
    )
    {
        var request = new RemoveTransactionCommand(UserId, new TransactionId(transactionId));
        var result = await mediator.Send(request, cancellationToken);

        return result.IsSuccess ? NoContent() : BadRequest(result.Failures);
    }


    [HttpPost("sync/{exchangeNamesName}")]
    [ProducesResponseType(typeof(PortfolioTransactionsDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> SyncWithExchangeAsync(
        CryptoExchangeNames exchangeNamesName,
        CancellationToken cancellationToken
    )
    {
        var request = new SyncWithExchangeCommand(UserId, (CryptoExchangeName)exchangeNamesName);
        var result = await mediator.Send(request, cancellationToken);
        var dto = result.Adapt<PortfolioTransactionsDto>();

        return Ok(dto);
    }
}