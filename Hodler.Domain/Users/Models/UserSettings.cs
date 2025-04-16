using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Users.Models;

public class UserSettings
{
    public Guid UserSettingsId { get; }
    public UserId UserId { get; }
    public FiatCurrency Currency { get; }
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
        Currency = currency ?? FiatCurrency.UsDollar;
        Theme = theme ?? AppTheme.Dark;
    }
}