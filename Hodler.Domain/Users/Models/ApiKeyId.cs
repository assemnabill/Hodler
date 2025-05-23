using Hodler.Domain.Shared.Aggregate;

namespace Hodler.Domain.Users.Models;

public class ApiKeyId : PrimitiveWrapper<Guid, ApiKeyId>
{
    public ApiKeyId(Guid value) : base(value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException($"Invalid {nameof(ApiKeyId)}");
        }
    }
}