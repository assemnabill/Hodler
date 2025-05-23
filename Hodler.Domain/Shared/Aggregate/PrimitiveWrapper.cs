using System.Diagnostics;

namespace Hodler.Domain.Shared.Aggregate;

[DebuggerDisplay("{Value}")]
public abstract class PrimitiveWrapper<TValue, TDerivedClass>(TValue value)
    : IEquatable<TDerivedClass>, IFormattable
    where TValue : struct
    where TDerivedClass : PrimitiveWrapper<TValue, TDerivedClass>
{
    public TValue Value { get; } = value;

    public bool Equals(TDerivedClass? other) => other is not null && EqualityComparer<TValue>.Default.Equals(Value, other.Value);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        if (Value is IFormattable formattableValue)
            return formattableValue.ToString(format, formatProvider);

        return ToString();
    }

    public static implicit operator TValue?(PrimitiveWrapper<TValue, TDerivedClass> primitiveWrapper) => primitiveWrapper?.Value;
    public static implicit operator TValue(PrimitiveWrapper<TValue, TDerivedClass> primitiveWrapper) => primitiveWrapper.Value;

    public static bool operator ==(PrimitiveWrapper<TValue, TDerivedClass> left, PrimitiveWrapper<TValue, TDerivedClass> right) =>
        EqualityComparer<PrimitiveWrapper<TValue, TDerivedClass>>.Default.Equals(left, right);

    public static bool operator !=(PrimitiveWrapper<TValue, TDerivedClass> left, PrimitiveWrapper<TValue, TDerivedClass> right) => !(left == right);

    public override bool Equals(object? obj) => Equals(obj as TDerivedClass);

    public override int GetHashCode() => HashCode.Combine(Value);

    public override string ToString() => $"{Value}";
}