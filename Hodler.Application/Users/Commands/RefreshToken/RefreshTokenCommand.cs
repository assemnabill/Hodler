using Hodler.Domain.Token.Models;
using Hodler.Domain.Token.Services;
using Hodler.Domain.Users.AuthenticationResult;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hodler.Application.Users.Commands.RefreshToken;

public class RefreshTokenCommand : IRequest<LoginResult?>
{
    public required string RefreshToken { get; init; } 
    public required string UserId { get; init; }
}

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResult?>
{
    private readonly IServiceProvider _serviceProvider;

    public RefreshTokenCommandHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task<LoginResult?> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userRepository = _serviceProvider.GetRequiredService<IUserRepository>();
        var isValidId = Guid.TryParse(request.UserId, out var userIdGuid);
        var userId = new UserId(userIdGuid);
        var isValid = await userRepository.IsRefreshTokenValidAsync(request.RefreshToken,userId ,cancellationToken);
        if(!isValid)
            return null;
        
        var tokenService = _serviceProvider.GetRequiredService<ITokenService>();
        var jwtSettings = _serviceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;
        var user = await userRepository.FindByAsync(userId, cancellationToken);
        if(user == null)
            return null;
        var token =  tokenService.GenerateJwtToken(user);
        var refreshToken = tokenService.GenerateRefreshToken();
        var refreshTokenExpirationDate = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationInDays);
        var updated = await userRepository.UpdateRefreshTokenAsync(request.RefreshToken,refreshToken, userId, refreshTokenExpirationDate,
            cancellationToken);
        if(!updated)
            return null;
        return new LoginResult
        {
            Token = token,
            TokenExpiresInByMinutes = jwtSettings.ExpirationInMinutes,
            RefreshToken = refreshToken,
            RefreshTokenExpiresInByDays = jwtSettings.RefreshTokenExpirationInDays,
            UserId = user.Id.Value.ToString(),
            Email =  user.ContactInfo.Email.Value,
            UserName = user.ContactInfo.UserName.Value,
        };
    }
}