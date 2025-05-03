using Hodler.Integration.Repositories.Portfolios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.Portfolios.Context;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(x => x.TransactionId);

        builder
            .Property(x => x.TransactionId)
            .ValueGeneratedNever()
            .HasDefaultValueSql(SqlFunctions.NewId);

        builder
            .HasIndex(
                nameof(Transaction.Timestamp),
                nameof(Transaction.PortfolioId),
                nameof(Transaction.Type),
                nameof(Transaction.FiatAmount),
                nameof(Transaction.BtcAmount)
            );

    }

    private static class SqlFunctions
    {
        public const string NewId = "gen_random_uuid()";
    }
}