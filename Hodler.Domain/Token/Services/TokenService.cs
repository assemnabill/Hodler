using Hodler.Domain.Token.Models;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hodler.Domain.Token.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthenticationRepository _authenticationRepository;

        public TokenService(IOptions<JwtSettings> jwtSettings, IAuthenticationRepository authenticationRepository)
        {
            _jwtSettings = jwtSettings.Value;
            _authenticationRepository = authenticationRepository;
        }

        public async Task<string> GenerateJwtTokenAsync(IUser user)
        {
            var tokenHnadler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti , Guid.CreateVersion7().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName,user.ContactInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email,user.ContactInfo.Email),
                new Claim(JwtRegisteredClaimNames.PhoneNumber , user.ContactInfo.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64)
            };
            //TODO Check If There Is Role
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            var token = tokenHnadler.CreateToken(tokenDescriptor);
            return await Task.FromResult(tokenHnadler.WriteToken(token));
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (!tokenHandler.CanReadToken(token))
                throw new SecurityTokenException("Invalid token format");
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                LogValidationExceptions = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(_jwtSettings.ClockSkewMinutes)
            };
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            await ValidateCustomClaimsAsync(principal);

            return principal;
        }


        private async Task ValidateCustomClaimsAsync(ClaimsPrincipal principal)
        {
            var sub = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
            var userName = principal.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
            var iat = principal.FindFirst(JwtRegisteredClaimNames.Iat)?.Value;
            if (string.IsNullOrEmpty(sub))
                throw new SecurityTokenException("Token missing subject claim");

            if (string.IsNullOrEmpty(email))
                throw new SecurityTokenException("Token missing email claim");

            if (string.IsNullOrEmpty(iat))
                throw new SecurityTokenException("Token missing issued-at claim");

            if (long.TryParse(iat, out var iatUnixTime))
            {
                var issuedAt = DateTimeOffset.FromUnixTimeSeconds(iatUnixTime);
                if (issuedAt > DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.ClockSkewMinutes)) // 5 min clock skew
                {
                    throw new SecurityTokenException("Token issued in future");
                }
            }
            if (!await _authenticationRepository.IsExistUserAsync(userName, email))
            {
                throw new SecurityTokenException("User no longer exists");
            }
        }


    }
}
