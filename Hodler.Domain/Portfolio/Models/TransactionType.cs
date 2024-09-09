using System.Runtime.Serialization;

namespace Hodler.Domain.Portfolio.Models;

public enum TransactionType {
    [EnumMember(Value = "buy")] Buy,
    [EnumMember(Value = "sell")] Sell,
    [EnumMember(Value = "unknown")] Unknown
}