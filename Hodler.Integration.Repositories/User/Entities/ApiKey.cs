namespace Hodler.Integration.Repositories.User.Entities;

public class ApiKey : Entity
{
    public Guid ApiKeyId { get; init; }
    public string UserId { get; init; }
    public string ApiKeyName { get; init; }
    public string Value { get; init; }
    public string? Secret { get; init; }
}