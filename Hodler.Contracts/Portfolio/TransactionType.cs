using System.Runtime.Serialization;

namespace Hodler.Contracts.Portfolio;

public enum TransactionType {
    [EnumMember(Value = "buy")] Buy,
    [EnumMember(Value = "sell")] Sell,
    [EnumMember(Value = "unknown")] Unknown
}