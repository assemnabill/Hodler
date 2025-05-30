using Hodler.Domain.Token.Services;
using Hodler.Domain.Users.AuthenticationResult;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;

namespace Hodler.Domain.Users.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IAuthenticationRepository authenticationRepository,
                                     ITokenService tokenService)
        {
            _authenticationRepository = authenticationRepository;
            _tokenService = tokenService;
        }


        #region RegisterLogin
        public async Task<RegisterResult> CreateUserAsync(string email, string password,
                                 string userName, string phoneNumber,
                                 CancellationToken cancellationToken)
        {
            var registerResult = new RegisterResult { Succeeded = false };
            var isExist = await _authenticationRepository.IsExistUserAsync(userName, email);
            if (isExist)
            {
                registerResult.IsExistUser = true;
                return registerResult;
            }
            var userId = new UserId(Guid.CreateVersion7());
            var user = new User(userId, null, null);
            user.AddContactInfo(userName, phoneNumber, email);
            return await _authenticationRepository.RegisterAsync(user, password);
        }


        public async Task<LoginResult> LoginAsync(string userNameOrEmail, string password, CancellationToken cancellationToken)
        {
            var loginResult = new LoginResult
            {
                Succeeded = false,
                Error = "Invalid user Or Password"
            };
            var user = await _authenticationRepository.FindByEmailAsync(userNameOrEmail, cancellationToken)
                      ?? await _authenticationRepository.FindByUserNameAsync(userNameOrEmail, cancellationToken);
            if (user is null)
                return loginResult;
            loginResult = await _authenticationRepository.LoginAsync(user, password);
            if (!loginResult.Succeeded)
                return loginResult;
            loginResult.Token = await _tokenService.GenerateJwtTokenAsync(user);
            loginResult.Succeeded = true;
            loginResult.Error = null;
            return loginResult;
        }

        #endregion

        #region GenerateToken
        public async Task<string?> GenerateConfirmEmailRequestToken(string email)
        {
            var isExistUser = await _authenticationRepository.IsExistUserAsync(email);
            if (isExistUser)
            {
                return await _authenticationRepository.GenerateConfirmEmailTokenAsync(email);
            }
            return null;
        }
        public async Task<string?> GenerateResetPasswordRequestToken(string email)
        {
            var isExistUser = await _authenticationRepository.IsExistUserAsync(email);
            if (isExistUser)
            {
                return await _authenticationRepository.GenerateResetPasswordTokenAsync(email);
            }
            return null;
        }

        #endregion
        public async Task<bool> ConfirmEmailAsync(string email, string token)
        {
            return await _authenticationRepository.ConfirmEmailAsync(email, token);
        }
        public async Task<bool> ResetPasswordAsync(string email, string newPassword, string token)
        {
            var isExist = await _authenticationRepository.IsExistUserAsync(email);
            if (!isExist)
                return false;
            return await _authenticationRepository.ResetPasswordAsync(email, newPassword, token);
        }
    }
}
