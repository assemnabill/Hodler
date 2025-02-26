using Corz.DomainDriven.Abstractions.Exceptions;
using Hodler.Application.User.Commands.AddApiKey;
using Hodler.Contracts.User.AddApiKey;
using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.User.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.UserScope.User;

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
}