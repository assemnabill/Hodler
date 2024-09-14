using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hodler.Integration.Repositories.User.Entities;

public class UserSettings : Entity
{
    public Guid UserSettingsId { get; init; }
    public Guid UserId { get; init; }
    public string? Language { get; init; }
    public string? Currency { get; init; }
    public string? Theme { get; init; }
    public string? Region { get; init; }
}