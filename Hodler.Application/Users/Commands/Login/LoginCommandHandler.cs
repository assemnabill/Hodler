using Hodler.Domain.Token.Models;
using Hodler.Domain.Token.Services;
using Hodler.Domain.Users.AuthenticationResult;
using Hodler.Domain.Users.Ports;
using Hodler.Domain.Users.Services;
using Hodler.Domain.Users.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Hodler.Application.Users.Commands.Login
{
    public class LoginCommandHandler(IUserRepository userRepository ,ITokenService tokenService ,
                                     IOptions<JwtSettings> jwtSettings) 
                                    : IRequestHandler<LoginCommand, LoginResult?>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly JwtSettings _jwtSettings = jwtSettings.Value;
        public async Task<LoginResult?> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var isEmail = EmailAddress.TryCreate(request.UserNameOrEmail, out var email);
            var user = isEmail
                           ? await _userRepository.CheckLoginCredentialsAsync(email, request.Password, cancellationToken)
                       : UserName.TryCreate(request.UserNameOrEmail, out var userName)
                           ? await _userRepository.CheckLoginCredentialsAsync(userName, request.Password, cancellationToken)
                           : null;
            if (user is null)
                return null;
            var token = _tokenService.GenerateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
            await _userRepository.StoreRefreshTokenAsync(refreshToken, refreshTokenExpirationDate, user.Id , cancellationToken);
            return new LoginResult
            {
                Token = token,
                TokenExpiresInByMinutes = _jwtSettings.ExpirationInMinutes,
                RefreshToken = refreshToken,
                RefreshTokenExpiresInByDays = _jwtSettings.RefreshTokenExpirationInDays,
                UserId = user.Id.Value.ToString(),
                Email =  user.ContactInfo.Email.Value,
                UserName = user.ContactInfo.UserName.Value,
            };
        }
    }
}
