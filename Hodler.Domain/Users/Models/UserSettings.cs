using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Users.Models;

public class UserSettings
{
    public Guid UserSettingsId { get; }
    public UserId UserId { get; }
    public FiatCurrency DisplayCurrency { get; }
    public AppTheme Theme { get; }

    public UserSettings(
        Guid userSettingsId,
        UserId userId,
        FiatCurrency? currency = null,
        AppTheme? theme = null
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserSettingsId = userSettingsId;
        UserId = userId;
        DisplayCurrency = currency ?? FiatCurrency.UsDollar;
        Theme = theme ?? AppTheme.Dark;
    }

    public UserSettings ChangeDisplayCurrency(FiatCurrency newDisplayCurrency) => new(UserSettingsId, UserId, newDisplayCurrency, Theme);
}