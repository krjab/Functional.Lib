// ReSharper disable once CheckNamespace
namespace Option;

public readonly struct Some<T> : IEquatable<Some<T>>
{
	internal T Value { get; }
	internal Some(T value)
	{
		var isNullable = Nullable.GetUnderlyingType(typeof(T)) != null;
		
		if (value == null && !isNullable)
		{
			throw new ArgumentNullException(nameof(value));
		}
		Value = value;
	}

    public override bool Equals(object? obj)
    {
        return obj is Some<T> other && Value?.Equals(other.Value) == true;
    }

    public override int GetHashCode()
    {
        return Value?.GetHashCode() ?? 0;
    }

    public static bool operator ==(Some<T> left, Some<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Some<T> left, Some<T> right)
    {
        return !(left == right);
    }

    public bool Equals(Some<T> other)
    {
        return EqualityComparer<T>.Default.Equals(Value, other.Value);
    }
}