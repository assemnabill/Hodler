using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hodler.Integration.Repositories.User.Entities;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly JwtService _jwtService;
    private readonly IEmailService _emailService;

    public AuthController(UserManager<User> userManager, JwtService jwtService, IEmailService emailService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _emailService = emailService;
    }

    // POST /register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new User { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Send email confirmation
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token }, Request.Scheme);
        await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by clicking this link: {confirmationLink}");

        return Ok(new { Message = "User registered successfully. Please check your email to confirm your account." });
    }

    // POST /login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized();
        }

        if (!await _userManager.IsEmailConfirmedAsync(user))
        {
            return BadRequest("Email is not confirmed.");
        }

        var tokens = _jwtService.GenerateTokens(user);
        return Ok(new { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Unauthorized();
        }

        var tokens = _jwtService.GenerateTokens(user);
        return Ok(new { AccessToken = tokens.AccessToken, RefreshToken = tokens.RefreshToken });
    }

    [HttpGet("confirmEmail")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Email confirmed successfully.");
    }

    [HttpPost("resendConfirmationEmail")]
    public async Task<IActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (await _userManager.IsEmailConfirmedAsync(user))
        {
            return BadRequest("Email is already confirmed.");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var confirmationLink = Url.Action("ConfirmEmail", "Auth", new { userId = user.Id, token }, Request.Scheme);
        await _emailService.SendEmailAsync(user.Email, "Confirm your email", $"Please confirm your account by clicking this link: {confirmationLink}");

        return Ok("Confirmation email sent successfully.");
    }

    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = Url.Action("ResetPassword", "Auth", new { email = user.Email, token }, Request.Scheme);
        await _emailService.SendEmailAsync(user.Email, "Reset your password", $"Please reset your password by clicking this link: {resetLink}");

        return Ok("Password reset email sent successfully.");
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Password reset successfully.");
    }

    [HttpGet("manage/info")]
    [Authorize]
    public async Task<IActionResult> GetUserInfo()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        return Ok(new { user.Email, user.UserName });
    }

    [HttpPost("manage/info")]
    [Authorize]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserInfoRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        user.Email = request.Email;
        user.UserName = request.UserName;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("User information updated successfully.");
    }
}

public interface IEmailService
{
    Task SendEmailAsync(string? userEmail, string confirmYourEmail, string s);
}