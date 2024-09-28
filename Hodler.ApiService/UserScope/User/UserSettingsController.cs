using Corz.DomainDriven.Abstractions.Exceptions;
using Hodler.Application.User.Commands.AddApiKey;
using Hodler.Contracts.User.AddApiKey;
using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.User.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.UserScope.User;

[ApiController]
[Route("api/[controller]")]
public class UserSettingsController : ControllerBase
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
                    Enum.Parse<ApiKeyName>(addApiKeyRequestContract.ApiName),
                    addApiKeyRequestContract.ApiKey,
                    new UserId(addApiKeyRequestContract.UserId)
            );

            var success = await _mediator.Send(request, cancellationToken);

            return Ok(success);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    private IActionResult HandleException(Exception exception) =>
        exception switch
        {
            DomainException domainException => BadRequest(domainException.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
}