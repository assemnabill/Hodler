using Hodler.Integration.Repositories.User.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.User.Context;

public class UserDbContext(DbContextOptions<UserDbContext> options) :
    IdentityDbContext<Entities.User>(options)
{
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<UserSettings> ApiKeys => Set<UserSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUser(modelBuilder.Entity<Entities.User>());
        ConfigureUserSettings(modelBuilder.Entity<UserSettings>());
        ConfigureApiKey(modelBuilder.Entity<ApiKey>());
    }

    private void ConfigureUserSettings(EntityTypeBuilder<UserSettings> modelBuilder)
    {
        modelBuilder.HasKey(x => x.UserSettingsId);
    }

    private void ConfigureApiKey(EntityTypeBuilder<ApiKey> modelBuilder)
    {
        modelBuilder.HasKey(x => x.ApiKeyId);
    }

    private static void ConfigureUser(EntityTypeBuilder<Entities.User> modelBuilder)
    {
        modelBuilder.HasOne(x => x.UserSettings);
        modelBuilder.HasMany(x => x.ApiKeys);
    }
}