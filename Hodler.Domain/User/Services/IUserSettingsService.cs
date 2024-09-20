using Hodler.Domain.User.Models;

namespace Hodler.Domain.User.Services;

public interface IUserSettingsService
{
    Task<bool> AddApiKeyAsync(ApiName apiName, string value, UserId userId, CancellationToken cancellationToken);
}