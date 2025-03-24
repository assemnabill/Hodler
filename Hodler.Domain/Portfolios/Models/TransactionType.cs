using System.Runtime.Serialization;

namespace Hodler.Domain.Portfolios.Models;

public enum TransactionType
{
    [EnumMember(Value = "buy")] Buy = 1,
    [EnumMember(Value = "sell")] Sell = 2,
    [EnumMember(Value = "unknown")] Unknown = 3
}