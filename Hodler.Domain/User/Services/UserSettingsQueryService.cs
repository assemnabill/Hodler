using Hodler.Domain.User.Models;
using Hodler.Domain.User.Ports;

namespace Hodler.Domain.User.Services;

public class UserSettingsQueryService : IUserSettingsQueryService
{
    private readonly IUserRepository _userRepository;

    public UserSettingsQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApiKey?> GetApiKeyAsync(UserId userId, ApiName apiName, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(userId);

        var user = await _userRepository.FindByAsync(userId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"User with id {userId} not found.");
        

        var apiKey = user.ApiKeys.FirstOrDefault(x => x.ApiName == apiName);

        return apiKey;
    }
}