using Hodler.Domain.Users.Models;
using System.Security.Claims;

namespace Hodler.Domain.Token.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateJwtTokenAsync(IUser user);
        public Task<ClaimsPrincipal> ValidateTokenAsync(string token);
    }
}
