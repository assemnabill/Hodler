namespace Hodler.Domain.Token.Models
{
    public class JwtSettings
    {
        public string Secret { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpirationInMinutes { get; set; }
        public int ClockSkewMinutes { get; set; } = 5;
        public int RefreshTokenExpirationInDays { get; set; }
    }
}
