using Microsoft.AspNetCore.Identity;

namespace Hodler.Integration.Repositories.Users.Entities;

public class User : IdentityUser
{
    public virtual UserSettings UserSettings { get; set; }
    public virtual ICollection<ApiKey> ApiKeys { get; set; }
}