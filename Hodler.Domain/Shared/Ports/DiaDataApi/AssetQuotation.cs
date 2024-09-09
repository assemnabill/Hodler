namespace Hodler.Domain.Shared.Ports.DiaDataApi;

public record AssetQuotation (
    string Symbol,
    string Name,
    string Address,
    string Blockchain,
    double Price,
    double PriceYesterday,
    double VolumeYesterdayUSD,
    DateTime Time,
    string Source,
    string Signature
);