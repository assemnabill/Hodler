using Hodler.Application.Users.Commands.ConfirmEmail;
using Hodler.Application.Users.Commands.ConfirmEmailRequest;
using Hodler.Application.Users.Commands.Login;
using Hodler.Application.Users.Commands.RefreshToken;
using Hodler.Application.Users.Commands.Register;
using Hodler.Application.Users.Commands.ResetPassword;
using Hodler.Application.Users.Commands.RestPasswordRequest;
using Hodler.Contracts.Users;
using Hodler.Contracts.Users.Authantecation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hodler.ApiService.Users
{
    [AllowAnonymous]
    public class AuthController : ApiController
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel registerModel, CancellationToken cancellationToken)
        {
            var request = registerModel.Adapt<RegisterCommand>();
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Succeeded)
                return CreatedAtAction("Login", null);
            return BadRequest(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync(LoginModel loginModel, CancellationToken cancellationToken)
        {
            var request = new LoginCommand { UserNameOrEmail = loginModel.EmailOrUserName, Password = loginModel.Password };
            var result = await _mediator.Send(request, cancellationToken);
            if (result is null)
                return Unauthorized("Invalid Credentials");
            Response.Cookies.Append(
                "jwt",
                result.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Ensure this is true in production (HTTPS only)
                    SameSite = SameSiteMode.Strict, // or Lax if needed for cross-site
                    Expires = DateTime.UtcNow.AddMinutes(result.TokenExpiresInByMinutes), // Short expiration
                    Path = "/" // Accessible across the entire site
                });
            Response.Cookies.Append(
                "RefreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Ensure this is true in production (HTTPS only)
                    SameSite = SameSiteMode.Strict, // or Lax if needed for cross-site
                    Expires = DateTime.UtcNow.AddDays(result.RefreshTokenExpiresInByDays), // Short expiration
                    Path = "/" // Accessible across the entire site
                });
            var authResult = new AuthResult
            {
                Email = result.Email,
                UserName = result.UserName,
                UserId = result.UserId
            };
            return Ok(authResult);
        }
        [HttpPost("ResetPasswordRequest")]
        public async Task<IActionResult> ResetPasswordRequest(RestPasswordRequestCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result)
                return Ok();
            return NotFound("Invalid Email");
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand resetPassword, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(resetPassword, cancellationToken);
            if (result)
                return Ok();
            return BadRequest("Invalid Email Or Token");
        }
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
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string email, string token, CancellationToken cancellationToken)
        {
            var confirmEmailRequest = new ConfirmEmailCommand { Email = email, Token = token };
            var result = await _mediator.Send(confirmEmailRequest, cancellationToken);
            if (result)
                return Ok();
            return NotFound("Invalid Email Or Token");
        }

        [HttpPost("Logout")]
        public  IActionResult Logout(CancellationToken cancellationToken)
        {
            var token = Request.Cookies["jwt"];
            var refreshToken = Request.Cookies["RefreshToken"];
            Response.Cookies.Delete("jwt");
            Response.Cookies.Delete("RefreshToken");
            return Ok();
        }
        [HttpPost("RefreshToken/{userId}")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> RefreshTokenAsync(string userId, CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies["RefreshToken"];
            if (refreshToken is null)
                return Unauthorized();
            var request = new RefreshTokenCommand { RefreshToken = refreshToken, UserId = userId };
            var result = await _mediator.Send(request, cancellationToken);
            if (result is null)
                return Unauthorized("Invalid Credentials");
            Response.Cookies.Append(
                "jwt",
                result.Token,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Ensure this is true in production (HTTPS only)
                    SameSite = SameSiteMode.Strict, // or Lax if needed for cross-site
                    Expires = DateTime.UtcNow.AddMinutes(result.TokenExpiresInByMinutes), // Short expiration
                    Path = "/" // Accessible across the entire site
                });
            Response.Cookies.Append(
                "RefreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Ensure this is true in production (HTTPS only)
                    SameSite = SameSiteMode.Strict, // or Lax if needed for cross-site
                    Expires = DateTime.UtcNow.AddDays(result.RefreshTokenExpiresInByDays), // Short expiration
                    Path = "/" // Accessible across the entire site
                });
            var authResult = new AuthResult
            {
                Email = result.Email,
                UserName = result.UserName,
                UserId = result.UserId
            };
            return Ok(authResult);
        }

    }
}
