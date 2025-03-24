namespace Hodler.Integration.Repositories.Users.Entities;

public class UserSettings : Entity
{
    public Guid UserSettingsId { get; init; }
    public string UserId { get; init; }
    public string? Language { get; init; }
    public string? Currency { get; init; }
    public string? Theme { get; init; }
    public string? Region { get; init; }
}