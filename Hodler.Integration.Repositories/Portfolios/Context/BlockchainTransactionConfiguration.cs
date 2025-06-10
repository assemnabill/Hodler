using Hodler.Integration.Repositories.Portfolios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.Portfolios.Context;

public class BlockchainTransactionConfiguration : IEntityTypeConfiguration<BlockchainTransaction>
{
    public void Configure(EntityTypeBuilder<BlockchainTransaction> builder)
    {
        builder.HasKey(x => x.TransactionHash);

        builder
            .HasIndex(
                nameof(BlockchainTransaction.Timestamp),
                nameof(BlockchainTransaction.Type),
                nameof(BlockchainTransaction.FiatValueInUsd),
                nameof(BlockchainTransaction.BtcAmount),
                nameof(BlockchainTransaction.FromAddress),
                nameof(BlockchainTransaction.ToAddress),
                nameof(BlockchainTransaction.Status)
            );

    }
}