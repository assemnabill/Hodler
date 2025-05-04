using Hodler.Integration.Repositories.BitcoinPrices.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.BitcoinPrices.Context;

internal class BitcoinPriceConfiguration : IEntityTypeConfiguration<BitcoinPrice>
{
    public void Configure(EntityTypeBuilder<BitcoinPrice> builder)
    {
        builder
            .HasKey(x => new { x.Date, x.Currency });

        builder
            .HasIndex(
                nameof(BitcoinPrice.Close),
                nameof(BitcoinPrice.High),
                nameof(BitcoinPrice.Low),
                nameof(BitcoinPrice.Open)
            );
    }
}