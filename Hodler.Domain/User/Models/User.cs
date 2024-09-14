using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.User.Models;

public class User : AggregateRoot<User>, IUser
{
    public UserId UserId { get; }
    public UserSettings UserSettings { get; }
    public IReadOnlyCollection<ApiKey>? ApiKeys { get; }

    public User(
        UserId userId,
        UserSettings userSettings,
        IReadOnlyCollection<ApiKey>? apiKeys = null
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        UserId = userId;
        UserSettings = userSettings;
        ApiKeys = apiKeys;
    }
}