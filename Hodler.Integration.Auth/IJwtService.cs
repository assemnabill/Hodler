using System.Security.Claims;
using Hodler.Integration.Repositories.User.Entities;

namespace Hodler.Integration.Auth;

public interface IJwtService
{
    AuthenticationTokens GenerateTokens(User user);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}