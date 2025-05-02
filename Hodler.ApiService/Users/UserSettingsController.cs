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
public class UserSettingsController : ApiController
{
    private readonly IMediator _mediator;

    public UserSettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("apiKey")]
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

            var success = await _mediator.Send(request, cancellationToken);

            return Ok(success);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    [HttpPost("changeDisplayCurrency")]
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

            var success = await _mediator.Send(request, cancellationToken);

            return Ok(success);

        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    [HttpGet("accountSettings")]
    [ProducesResponseType(typeof(AccountSettingsContract), 200)]
    public async Task<IActionResult> GetUserAccountSettingsAsync(CancellationToken cancellationToken)
    {
        try
        {
            var request = new UserAccountSettingsQuery(UserId);

            var success = await _mediator.Send(request, cancellationToken);
            var dto = success.Adapt<AccountSettingsContract>();

            return Ok(dto);

        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
}