namespace Hodler.Integration.Repositories.BitcoinPrices.Entities;

public class BitcoinPrice : Entity
{
    public DateOnly Date { get; set; }
    public int Currency { get; set; }
    public decimal Close { get; set; }
    public decimal? Open { get; set; }
    public decimal? High { get; set; }
    public decimal? Low { get; set; }
    public decimal? Volume { get; set; }
}