using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.CryptoExchanges.Models;
using Hodler.Domain.Shared.Models;

namespace Hodler.Domain.Users.Models;

public interface IUser : IAggregateRoot<IUser>
{
    UserId Id { get; }
    UserSettings UserSettings { get; }
    IReadOnlyCollection<ApiKey> ApiKeys { get; }

    bool AddApiKey(ApiKeyName apiKeyName, string value, string? secret);
    bool ChangeDisplayCurrency(FiatCurrency newDisplayCurrency);
}