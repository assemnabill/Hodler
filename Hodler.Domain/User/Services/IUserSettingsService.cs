using Hodler.Domain.CryptoExchange.Models;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.User.Services;

public interface IUserSettingsService
{
    Task<bool> AddApiKeyAsync(ApiKeyName apiKeyName, string value, UserId userId, CancellationToken cancellationToken);
}