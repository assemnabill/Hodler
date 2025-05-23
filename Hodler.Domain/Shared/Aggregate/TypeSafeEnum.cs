using System.Reflection;

namespace Hodler.Domain.Shared.Aggregate;

public abstract class TypeSafeEnum<TType>
    : IComparable<TypeSafeEnum<TType>>, IEquatable<TypeSafeEnum<TType>>
    where TType : TypeSafeEnum<TType>
{
    public int Id { get; }

    protected static Lazy<IReadOnlyDictionary<int, TType>> Map { get; } = new(() =>
        {
            return typeof(TType)
                .GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(prop => prop.PropertyType == typeof(TType))
                .Select(prop => (TType)prop.GetValue(null)!)
                .ToDictionary(x => x.Id, x => x);
        }
    );

    protected TypeSafeEnum(int id)
    {
        Id = id;
    }

    public int CompareTo(TypeSafeEnum<TType>? other) => other is null ? 1 : Id.CompareTo(other.Id);

    public bool Equals(TypeSafeEnum<TType>? other) => other is not null && Id == other.Id;

    public static bool operator >(TypeSafeEnum<TType> operand1, TypeSafeEnum<TType> operand2) => operand1.CompareTo(operand2) == 1;

    public static bool operator <(TypeSafeEnum<TType> operand1, TypeSafeEnum<TType> operand2) => operand1.CompareTo(operand2) == -1;

    public static bool operator >=(TypeSafeEnum<TType> operand1, TypeSafeEnum<TType> operand2) => operand1.CompareTo(operand2) >= 0;

    public static bool operator <=(TypeSafeEnum<TType> operand1, TypeSafeEnum<TType> operand2) => operand1.CompareTo(operand2) <= 0;

    public static IEnumerable<TType> AsEnumerable() => Map.Value.Values;
    public static TType FromId(int id) => Map.Value[id];

    public override bool Equals(object? obj) => Equals(obj as TypeSafeEnum<TType>);

    public override int GetHashCode() => Id.GetHashCode();

    public override string ToString() => Id.ToString();
}