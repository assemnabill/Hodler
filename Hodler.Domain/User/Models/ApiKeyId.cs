using Corz.DomainDriven.Abstractions.Models.Bases;

namespace Hodler.Domain.User.Models;

public class ApiKeyId : PrimitiveWrapper<Guid, ApiKeyId>
{
    public ApiKeyId(Guid value) : base(value)
    {
        if (value == default)
        {
            throw new ArgumentException($"Invalid {nameof(ApiKeyId)}");
        }
    }
}