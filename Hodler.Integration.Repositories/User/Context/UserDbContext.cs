using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hodler.Integration.Repositories.User.Context;

public class UserDbContext(DbContextOptions<UserDbContext> options) : 
    IdentityDbContext<Entities.User>(options)
{
}