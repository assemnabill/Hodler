using Hodler.Integration.Repositories.Portfolios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.Portfolios.Context;

public class BitcoinWalletConfiguration : IEntityTypeConfiguration<BitcoinWallet>
{
    public void Configure(EntityTypeBuilder<BitcoinWallet> builder)
    {
        builder.HasKey(x => x.BitcoinWalletId);

        builder
            .Property(x => x.BitcoinWalletId)
            .ValueGeneratedNever()
            .HasDefaultValueSql(SqlFunctions.NewId);

        builder
            .HasIndex(x => x.Address)
            .IsUnique();

        builder
            .HasOne(x => x.Portfolio)
            .WithMany(x => x.BitcoinWallets)
            .HasForeignKey(x => x.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static class SqlFunctions
    {
        public const string NewId = "gen_random_uuid()";
    }
}