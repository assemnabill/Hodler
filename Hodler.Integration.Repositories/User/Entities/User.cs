using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Hodler.Integration.Repositories.User.Entities;

public class User : IdentityUser
{
    [ForeignKey(nameof(Entities.UserSettings))]
    public Guid UserSettingsId { get; set; }

    public virtual UserSettings UserSettings { get; set; }
    public virtual ICollection<ApiKey> ApiKeys { get; set; }
}