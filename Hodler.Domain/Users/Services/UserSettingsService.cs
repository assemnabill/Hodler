using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Users.Models;
using Hodler.Domain.Users.Ports;

namespace Hodler.Domain.Users.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly IUserRepository _userRepository;

    public UserSettingsService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserSettings> GetUserSettingsAsync(
        UserId userId,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        var user = await _userRepository.FindByAsync(userId, cancellationToken);

        if (user == null)
            throw new InvalidOperationException($"User with id {userId} not found.");

        return user.UserSettings;
    }

    public async Task<bool> AddApiKeyAsync(
        ApiKeyName apiKeyName,
        string value,
        UserId userId,
        string? secret,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(apiKeyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentNullException.ThrowIfNull(userId);

        var user = await _userRepository.FindByAsync(userId, cancellationToken);

        if (user == null)
            throw new InvalidOperationException($"User with id {userId} not found.");

        var changed = user.AddApiKey(apiKeyName, value, secret);

        if (changed)
            await _userRepository.StoreAsync(user, cancellationToken);

        return changed;
    }
}