namespace Hodler.Contracts.Users.Authantecation;

public class AuthResult
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string UserId { get; init; }
}