using Hodler.Domain.Shared.Aggregate;

namespace Hodler.Domain.Users.Models;

public class UserId : PrimitiveWrapper<Guid, UserId>
{
    public UserId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("Invalid user id");
        }
    }
}