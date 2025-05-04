using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;

namespace Hodler.Domain.Users.Services;

public class UserSettingsQueryService : IUserSettingsQueryService
{
    private readonly IUserRepository _userRepository;

    public UserSettingsQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiKey?> GetApiKeyAsync(UserId userId, ApiKeyName apiKeyName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var user = await _userRepository.FindByAsync(userId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"User with id {userId} not found.");


        var apiKey = user.ApiKeys.FirstOrDefault(x => x.ApiKeyName == apiKeyName);

        return apiKey;
    }

    public async Task<UserSettings> GetAccountSettings(UserId userId, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindByAsync(userId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"User with id {userId} not found.");

        return user.UserSettings;
    }
}