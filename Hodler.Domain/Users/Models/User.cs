using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Events;

namespace Hodler.Domain.Users.Models;

public class User : AggregateRoot<User>, IUser
{
    public UserId Id { get; }
    public UserSettings UserSettings { get; private set; }
    public IReadOnlyCollection<ApiKey>? ApiKeys { get; private set; }
    public ContactInfo ContactInfo { get; private set; }

    public User(
        UserId userId,
        UserSettings? userSettings,
        IReadOnlyCollection<ApiKey>? apiKeys = null
    )
    {
        ArgumentNullException.ThrowIfNull(userId);

        Id = userId;
        UserSettings = userSettings ?? new UserSettings(Guid.NewGuid(), userId);
        ApiKeys = apiKeys;
    }

    public bool AddApiKey(ApiKeyName apiKeyName, string value, string? secret)
    {
        ArgumentNullException.ThrowIfNull(apiKeyName);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);

        ApiKeys ??= new List<ApiKey>();
        var changed = false;

        if (ApiKeys.Any(x => x.ApiKeyName == apiKeyName && x.Value == value))
        {
            return changed;
        }

        var apiKey = new ApiKey(new ApiKeyId(Guid.NewGuid()), apiKeyName, value, Id, secret);

        ApiKeys = ApiKeys
            .Append(apiKey)
            .ToList();

        changed = true;
        return changed;
    }

    public bool ChangeDisplayCurrency(FiatCurrency newDisplayCurrency)
    {
        if (newDisplayCurrency.Id == UserSettings.DisplayCurrency.Id)
            return false;

        UserSettings = UserSettings.ChangeDisplayCurrency(newDisplayCurrency);
        EnqueueDomainEvent(new UserDisplayCurrencyChanged(newDisplayCurrency));

        return true;
    }

    public void AddContactInfo(string userName, string phoneNumber, string email)
    {
        ArgumentNullException.ThrowIfNull(userName);
        ArgumentNullException.ThrowIfNull(phoneNumber);
        ArgumentNullException.ThrowIfNull(email);
        ContactInfo = new ContactInfo(userName, phoneNumber, email);
    }
}