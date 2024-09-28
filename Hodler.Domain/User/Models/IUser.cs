using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.CryptoExchange.Models;

namespace Hodler.Domain.User.Models;

public interface IUser : IAggregateRoot<IUser>
{
    UserId Id { get; }
    UserSettings UserSettings { get; }
    IReadOnlyCollection<ApiKey> ApiKeys { get; }
    
    bool AddApiKey(ApiKeyName apiKeyName, string value);
}