using System.Runtime.Serialization;

namespace Hodler.Integration.ExternalApis.PriceCatalog.HistoricalBticoinPrice;

public enum CoinCapCandlesInterval
{
    [EnumMember(Value = "m1")] M1,
    [EnumMember(Value = "m5")] M5,
    [EnumMember(Value = "m15")] M15,
    [EnumMember(Value = "m30")] M30,
    [EnumMember(Value = "h1")] H1,
    [EnumMember(Value = "h2")] H2,
    [EnumMember(Value = "h4")] H4,
    [EnumMember(Value = "h8")] H8,
    [EnumMember(Value = "h12")] H12,
    [EnumMember(Value = "d1")] D1,
    [EnumMember(Value = "w1")] W1,
}