using Corz.DomainDriven.Abstractions.Models.Bases;
using Hodler.Domain.User.Models;

namespace Hodler.Domain.User.Ports;

public interface IUserRepository : IRepository<IUser>
{
    Task<IUser?> FindByAsync(
        UserId userId,
        CancellationToken cancellationToken
    );
}