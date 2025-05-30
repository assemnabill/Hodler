using Hodler.Domain.Users.AuthenticationResult;

namespace Hodler.Domain.Users.Services
{
    public interface IAuthenticationService
    {
        public Task<RegisterResult> CreateUserAsync(string email, string password, string userName
                                           , string phoneNumber, CancellationToken cancellationToken);
        public Task<LoginResult> LoginAsync(string userNameOrEmail, string password, CancellationToken cancellationToken);
        public Task<string?> GenerateResetPasswordRequestToken(string email);
        public Task<string?> GenerateConfirmEmailRequestToken(string email);
        public Task<bool> ConfirmEmailAsync(string email, string token);
        public Task<bool> ResetPasswordAsync(string email, string newPassword, string token);
    }
}
