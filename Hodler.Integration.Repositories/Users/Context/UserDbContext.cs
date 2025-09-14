using Hodler.Domain.Shared.Models;
using Hodler.Domain.Users.Models;
using Hodler.Integration.Repositories.Users.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ApiKey = Hodler.Integration.Repositories.Users.Entities.ApiKey;
using User = Hodler.Integration.Repositories.Users.Entities.User;
using UserSettings = Hodler.Integration.Repositories.Users.Entities.UserSettings;

namespace Hodler.Integration.Repositories.Users.Context;

public class UserDbContext(DbContextOptions<UserDbContext> options) :
    IdentityDbContext<User>(options)
{
    public DbSet<UserSettings> UserSettings => Set<UserSettings>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<UserRefreshTokens> UserRefreshTokens => Set<UserRefreshTokens>();

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

        modelBuilder.Property(x => x.Theme).HasDefaultValue(AppTheme.Dark);
        modelBuilder.Property(x => x.Currency).HasDefaultValue(FiatCurrency.UsDollar.Id);
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