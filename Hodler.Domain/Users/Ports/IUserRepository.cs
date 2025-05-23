using Hodler.Domain.Shared.Aggregate;
using Hodler.Domain.Users.Models;

namespace Hodler.Domain.Users.Ports;

public interface IUserRepository : IRepository<IUser>
{
    Task<IUser?> FindByAsync(
        UserId userId,
        CancellationToken cancellationToken
    );
}