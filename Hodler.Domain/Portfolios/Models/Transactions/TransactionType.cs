using System.Runtime.Serialization;

namespace Hodler.Domain.Portfolios.Models.Transactions;

public enum TransactionType
{
    [EnumMember(Value = "unknown")] Unknown = 0,
    [EnumMember(Value = "buy")] Buy = 1,
    [EnumMember(Value = "sell")] Sell = 2,
    [EnumMember(Value = "sent")] Sent = 3,
    [EnumMember(Value = "received")] Received = 4,
}