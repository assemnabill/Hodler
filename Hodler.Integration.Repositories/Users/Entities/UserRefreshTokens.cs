namespace Hodler.Integration.Repositories.Users.Entities;

public class UserRefreshTokens
{
    public required Guid Id { get; set; }
    public required string UserId { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime CreatedTime { get; set; }
    public required DateTime ExpDateTime { get; set; }
    public User? User { get; set; }
}