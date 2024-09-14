namespace Hodler.Domain.User.Models;

public class UserSettings
{
    public Guid UserSettingsId { get; }
    public UserId UserId { get; }
    public string? Language { get; }
    public string? Currency { get; }
    public string? Theme { get; }
    public string? Region { get; }

    public UserSettings(
        Guid userSettingsId,
        UserId userId,
        string? language,
        string? currency,
        string? theme,
        string? region)
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserSettingsId = userSettingsId;
        UserId = userId;
        Language = language;
        Currency = currency;
        Theme = theme;
        Region = region;
    }
}