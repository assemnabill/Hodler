using Hodler.Integration.Repositories.Portfolios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.Portfolios.Context;

public class ManualTransactionConfiguration : IEntityTypeConfiguration<ManualTransaction>
{
    public void Configure(EntityTypeBuilder<ManualTransaction> builder)
    {
        builder.HasKey(x => x.TransactionId);

        builder
            .Property(x => x.TransactionId)
            .ValueGeneratedNever()
            .HasDefaultValueSql(SqlFunctions.NewId);

        builder
            .HasIndex(
                nameof(ManualTransaction.Timestamp),
                nameof(ManualTransaction.PortfolioId),
                nameof(ManualTransaction.Type),
                nameof(ManualTransaction.FiatAmount),
                nameof(ManualTransaction.BtcAmount)
            );

        builder
            .HasOne(x => x.Portfolio)
            .WithMany(x => x.ManualTransactions)
            .HasForeignKey(x => x.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}