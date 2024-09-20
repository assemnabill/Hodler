using Hodler.Domain.User.Models;
using Hodler.Domain.User.Ports;

namespace Hodler.Domain.User.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly IUserRepository _userRepository;

    public UserSettingsService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> AddApiKeyAsync(
        ApiName apiName,
        string value,
        UserId userId,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(apiName);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        ArgumentNullException.ThrowIfNull(userId);

        var user = await _userRepository.FindByAsync(userId, cancellationToken);

        if (user == null)
            throw new InvalidOperationException($"User with id {userId} not found.");

        var changed = user.AddApiKey(apiName, value);

        if (changed)
            await _userRepository.StoreAsync(user, cancellationToken);

        return changed;
    }
}