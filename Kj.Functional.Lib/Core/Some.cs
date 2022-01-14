// ReSharper disable once CheckNamespace
namespace Option;

public struct Some<T>
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
}