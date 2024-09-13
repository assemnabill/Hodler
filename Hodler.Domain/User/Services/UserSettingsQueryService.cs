namespace Hodler.Domain.User.Services;

public class UserSettingsQueryService : IUserSettingsQueryService
{
    
    public Task<string> GetBitPandaApiKeyAsync(Guid userId, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var apiKey = "";
        return Task.FromResult(apiKey);
    }
}