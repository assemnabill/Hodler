using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.User.Models;

public class User : AggregateRoot<User>, IUser
{
    public UserId Id { get; }
    public UserSettings UserSettings { get; private set; }
    public IReadOnlyCollection<ApiKey>? ApiKeys { get; private set; }

    public bool AddApiKey(ApiName apiName, string value)
    {
        ArgumentNullException.ThrowIfNull(apiName);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        ApiKeys ??= new List<ApiKey>();

        if (ApiKeys.Any(x => x.ApiName == apiName && x.Value == value))
        {
            return false;
        }

        var apiKey = new ApiKey(new ApiKeyId(Guid.NewGuid()), apiName, value, Id);

        ApiKeys = ApiKeys
            .Append(apiKey)
            .ToList();

        return true;
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