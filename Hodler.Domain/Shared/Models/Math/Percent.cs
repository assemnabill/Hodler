using Hodler.Domain.Shared.Aggregate;

namespace Hodler.Domain.Shared.Models.Math;

public class Percent(uint value) : PrimitiveWrapper<uint, Percent>(value), IComparable<Percent>
{
    public decimal Fraction => Convert.ToDecimal(Value) / 100;

    public int CompareTo(Percent other) => Value.CompareTo(other.Value);

    public override string ToString() => $"{Value}%";
}