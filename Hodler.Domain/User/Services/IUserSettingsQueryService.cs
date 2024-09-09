namespace Hodler.Domain.User.Services;

public interface IUserSettingsQueryService
{
    Task<string> GetBitPandaApiKeyAsync(Guid userId, CancellationToken cancellationToken);
}