using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.User.Models;

public class UserId : PrimitiveWrapper<Guid, UserId>
{
    public UserId(Guid value) : base(value)
    {
        if (value == default)
        {
            throw new ArgumentException("Invalid user id");
        }
    }
}