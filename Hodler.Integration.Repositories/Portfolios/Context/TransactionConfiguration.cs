using Hodler.Integration.Repositories.Portfolios.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hodler.Integration.Repositories.Portfolios.Context;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(
            nameof(Transaction.PortfolioId),
            nameof(Transaction.Timestamp),
            nameof(Transaction.Type),
            nameof(Transaction.BtcAmount),
            nameof(Transaction.FiatAmount),
            nameof(Transaction.FiatCurrency),
            nameof(Transaction.MarketPrice),
            nameof(Transaction.CryptoExchange)
        );
    }
}