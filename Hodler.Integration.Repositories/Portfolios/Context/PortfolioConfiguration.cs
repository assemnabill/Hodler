using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.Portfolios.Context;

internal class PortfolioConfiguration : IEntityTypeConfiguration<Entities.Portfolio>
{
    public void Configure(EntityTypeBuilder<Entities.Portfolio> builder)
    {
        builder
            .HasMany(x => x.Transactions)
            .WithOne(x => x.Portfolio);
    }
}