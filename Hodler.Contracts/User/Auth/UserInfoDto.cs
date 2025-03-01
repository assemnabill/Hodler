public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RefreshTokenRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}

public class ResendConfirmationEmailRequest
{
    public string Email { get; set; }
}

public class ForgotPasswordRequest
{
    public string Email { get; set; }
}

public class ResetPasswordRequest
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}

public class UpdateUserInfoRequest
{
    public string Email { get; set; }
    public string UserName { get; set; }
}