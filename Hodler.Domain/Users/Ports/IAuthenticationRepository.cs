using Hodler.Domain.Users.AuthenticationResult;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Users.Ports
{
    public interface IAuthenticationRepository
    {
        public Task<IUser?> FindByEmailAsync(string email, CancellationToken cancellationToken);
        public Task<IUser?> FindByUserNameAsync(string userName, CancellationToken cancellationToken);
        public Task<bool> IsExistUserAsync(string? userName, string email);
        public Task<bool> IsExistUserAsync(string email);

        public Task<RegisterResult> RegisterAsync(IUser user, string password);
        public Task<LoginResult> LoginAsync(IUser user, string password);
        public Task<bool> ResetPasswordAsync(string email, string newPassword, string token);
        public Task<string> GenerateResetPasswordTokenAsync(string email);
        public Task<string> GenerateConfirmEmailTokenAsync(string email);
        public Task<bool> ConfirmEmailAsync(string email, string token);

    }
}
