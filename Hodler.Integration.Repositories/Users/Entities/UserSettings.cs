namespace Hodler.Integration.Repositories.Users.Entities;

public class UserSettings : Entity
{
    public Guid UserSettingsId { get; init; }
    public string UserId { get; init; }
    public int Currency { get; init; }
    public int Theme { get; init; }
}