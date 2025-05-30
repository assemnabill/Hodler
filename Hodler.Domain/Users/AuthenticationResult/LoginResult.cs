namespace Hodler.Domain.Users.AuthenticationResult
{
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
