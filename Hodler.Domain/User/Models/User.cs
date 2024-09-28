using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.CryptoExchange.Models;

namespace Hodler.Domain.User.Models;

public class User : AggregateRoot<User>, IUser
{
    public UserId Id { get; }
    public UserSettings UserSettings { get; private set; }
    public IReadOnlyCollection<ApiKey>? ApiKeys { get; private set; }

    public bool AddApiKey(ApiKeyName apiKeyName, string value)
    {
        ArgumentNullException.ThrowIfNull(apiKeyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        ApiKeys ??= new List<ApiKey>();
        var changed = false;

        if (ApiKeys.Any(x => x.ApiKeyName == apiKeyName && x.Value == value))
        {
            return changed;
        }

        var apiKey = new ApiKey(new ApiKeyId(Guid.NewGuid()), apiKeyName, value, Id);

        ApiKeys = ApiKeys
            .Append(apiKey)
            .ToList();

        changed = true;
        return changed;
    }


    public User(
        UserId userId,
        UserSettings userSettings,
        IReadOnlyCollection<ApiKey>? apiKeys = null
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        Id = userId;
        UserSettings = userSettings;
        ApiKeys = apiKeys;
    }
}