using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.User.Services;

public interface IUserSettingsQueryService
{
    Task<ApiKey?> GetApiKeyAsync(UserId userId, ApiKeyName apiKeyName, CancellationToken cancellationToken);
}