public class AuthenticationTokens
{
    public string AccessToken { get; }
    public string RefreshToken { get; }

    public AuthenticationTokens(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}