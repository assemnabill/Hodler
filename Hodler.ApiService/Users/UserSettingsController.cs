using Hodler.Application.Users.Commands.AddApiKey;
using Hodler.Application.Users.Commands.ChangeDisplayCurrency;
using Hodler.Application.Users.Queries.UserAccountSettings;
using Hodler.Contracts.Users;
using Hodler.Contracts.Users.AddApiKey;
using Hodler.Contracts.Users.ChangeDisplayCurrency;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.Users;

/// <summary>
/// A controller responsible for managing user-specific settings.
/// </summary>
public class UserSettingsController(IMediator mediator) : ApiController
{
    /// <summary>
    /// Adds a new API key for the user.
    /// </summary>
    /// <param name="addApiKeyRequestContract">The request data containing the API key name, API key value, and optional secret key.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the result of the add API key operation. Returns an <see cref="IActionResult"/> indicating success or failure.</returns>
    [HttpPost("apiKey")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddApiKey(
        [FromBody] AddApiKeyRequestContract addApiKeyRequestContract,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var request = new AddApiKeyCommand(
                Enum.Parse<ApiKeyName>(addApiKeyRequestContract.ApiKeyName),
                addApiKeyRequestContract.ApiKeyValue,
                UserId,
                addApiKeyRequestContract.Secret
            );

            var success = await mediator.Send(request, cancellationToken);

            return Ok(success);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    /// <summary>
    /// Changes the user's display currency to the specified new currency.
    /// </summary>
    /// <param name="contract">The request containing the new display currency to set.</param>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. Returns an <see cref="IActionResult"/> indicating success or failure.</returns>
    [HttpPost("changeDisplayCurrency")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeDisplayCurrencyAsync(
        [FromBody] ChangeDisplayCurrencyRequestContract contract,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var request = new ChangeDisplayCurrencyCommand(
                UserId,
                FiatCurrency.GetById((int)contract.NewDisplayCurrency)
            );

            var success = await mediator.Send(request, cancellationToken);

            return Ok(success);

        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    /// <summary>
    /// Retrieves the user account settings.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task representing the action result of the operation. Returns an <see cref="IActionResult"/> with the user's account settings on success, or an error status on failure.</returns>
    [HttpGet("accountSettings")]
    [ProducesResponseType(typeof(AccountSettingsContract), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserAccountSettingsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var request = new UserAccountSettingsQuery(UserId);
            var success = await mediator.Send(request, cancellationToken);
            var dto = success.Adapt<AccountSettingsContract>();

            return Ok(dto);

        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
}