namespace Hodler.Integration.Repositories.User.Entities;

public class ApiKey : Entity
{
    public Guid ApiKeyId { get; init; }
    public string UserId { get; init; }
    public string ApiName { get; init; }
    public string Key { get; init; }
}