using Hodler.Integration.Repositories.Users.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.Users.Context;

public class UserDbContext(DbContextOptions<UserDbContext> options) :
    IdentityDbContext<User>(options)
{
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureUser(modelBuilder.Entity<User>());
        ConfigureUserSettings(modelBuilder.Entity<UserSettings>());
        ConfigureApiKey(modelBuilder.Entity<ApiKey>());
    }

    private void ConfigureUserSettings(EntityTypeBuilder<UserSettings> modelBuilder)
    {
        modelBuilder.ToTable("UserSettings");
        modelBuilder.HasKey(x => x.UserSettingsId);
        modelBuilder.HasIndex(x => x.UserId);
    }

    private void ConfigureApiKey(EntityTypeBuilder<ApiKey> modelBuilder)
    {
        modelBuilder.ToTable("ApiKeys");
        modelBuilder.HasKey(x => x.ApiKeyId);
        modelBuilder.HasIndex(x => x.UserId);
    }

    private static void ConfigureUser(EntityTypeBuilder<User> modelBuilder)
    {
        modelBuilder.HasKey(x => x.Id);
        
        modelBuilder
            .HasOne(x => x.UserSettings)
            .WithOne()
            .HasForeignKey<UserSettings>(x => x.UserId);
        
        modelBuilder
            .HasMany(x => x.ApiKeys)
            .WithOne()
            .HasForeignKey(x => x.UserId);

        modelBuilder.HasMany(x => x.ApiKeys).WithOne();
    }
}