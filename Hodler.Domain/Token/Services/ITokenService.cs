using Hodler.Domain.Users.Models;
using System.Security.Claims;

namespace Hodler.Domain.Token.Services
{
    public interface ITokenService
    {
        string GenerateJwtToken(IUser user);
        string GenerateRefreshToken();
        Task<ClaimsPrincipal> ValidateTokenAsync(string token);
    }
}
