using Hodler.Domain.User.Models;

namespace Hodler.Domain.User.Services;

public interface IUserSettingsQueryService
{
    Task<ApiKey?> GetApiKeyAsync(UserId userId, ApiName apiName, CancellationToken cancellationToken);
}