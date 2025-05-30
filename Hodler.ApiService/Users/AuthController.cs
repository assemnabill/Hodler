using Hodler.Application.Users.Commands.ConfirmEmail;
using Hodler.Application.Users.Commands.ConfirmEmailRequest;
using Hodler.Application.Users.Commands.Login;
using Hodler.Application.Users.Commands.Register;
using Hodler.Application.Users.Commands.ResetPassword;
using Hodler.Application.Users.Commands.RestPasswordRequest;
using Hodler.Contracts.Users;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel registerModel, CancellationToken cancellationToken)
        {
            var request = registerModel.Adapt<RegisterCommand>();
            var result = await _mediator.Send(request, cancellationToken);
            if (result.IsExistUser)
                return NoContent();
            if (result.Succeeded)
                return CreatedAtAction("Login", null);
            return BadRequest(result);
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken)
        {
            var request = new LoginCommand { UserNameOrEmail = loginModel.EmailOrUserName, Password = loginModel.Password };
            var result = await _mediator.Send(request, cancellationToken);
            if (!result.Succeeded)
                return NotFound(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("ResetPasswordRequest")]
        public async Task<IActionResult> ResetPasswordRequest(RestPasswordRequestCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result)
                return Ok();
            return NotFound("Invalid Email");
        }
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand resetPassword, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(resetPassword, cancellationToken);
            if (result)
                return Ok();
            return BadRequest("Invalid Email Or Token");
        }
        [AllowAnonymous]
        [HttpPost("ConfirmEmailRequest")]
        public async Task<IActionResult> ConfirmEmailRequest(string email, CancellationToken cancellationToken)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var relativePath = Url.Action("ConfirmEmail");
            var resetUrl = $"{baseUrl}{relativePath}";
            var request = new ConfirmEmailRequestCommand { BaseUrl = resetUrl, Email = email };
            var result = await _mediator.Send(request, cancellationToken);
            if (result)
                return Ok();
            return NotFound("Invalid Email");
        }
        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token, CancellationToken cancellationToken)
        {
            var confiremEmailRequest = new ConfirmEmailCommand { Email = email, Token = token };
            var result = await _mediator.Send(confiremEmailRequest, cancellationToken);
            if (result)
                return Ok();
            return NotFound("Invalid Email Or Token");
        }

    }
}
