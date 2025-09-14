namespace Hodler.Domain.Users.AuthenticationResult
{
    public class LoginResult
    {
        public required string Token { get; init; }
        public required string RefreshToken { get; init; }
        public required int TokenExpiresInByMinutes { get; init; }
        public required int RefreshTokenExpiresInByDays { get; init; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }

    }
}
