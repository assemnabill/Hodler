using System.Security.Claims;
using Corz.DomainDriven.Abstractions.Exceptions;
using Hodler.Domain.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    protected UserId UserId => new(Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)!.Value));

    protected IActionResult HandleException(Exception exception) =>
        exception switch
        {
            DomainException domainException => BadRequest(domainException.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
}