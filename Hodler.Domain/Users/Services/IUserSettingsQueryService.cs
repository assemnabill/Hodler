using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Users.Services;

public interface IUserSettingsQueryService
{
    Task<ApiKey?> GetApiKeyAsync(
        UserId userId,
        ApiKeyName apiKeyName,
        CancellationToken cancellationToken = default
    );

    Task<UserSettings> GetAccountSettings(
        UserId userId,
        CancellationToken cancellationToken = default
    );
}