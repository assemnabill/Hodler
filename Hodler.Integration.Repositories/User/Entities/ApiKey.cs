using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hodler.Integration.Repositories.User.Entities;

public class ApiKey : Entity
{
    public Guid ApiKeyId { get; init; }
    public string ApiName { get; init; }
    public string Key { get; init; }
}