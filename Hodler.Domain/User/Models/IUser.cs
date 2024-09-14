using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.User.Models;

public interface IUser : IAggregateRoot<IUser>
{
    UserId UserId { get; }
    UserSettings UserSettings { get; }
    IReadOnlyCollection<ApiKey> ApiKeys { get; }
}