using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Users.Services;

public interface IUserSettingsService
{
    Task<UserSettings> GetUserSettingsAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    );

    Task<bool> AddApiKeyAsync(
        ApiKeyName apiKeyName,
        string value,
        UserId userId,
        string? secret,
        CancellationToken cancellationToken
    );
}