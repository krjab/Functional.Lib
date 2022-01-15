using Kj.Functional.Lib.Core;
using Option;

namespace Kj.Functional.Lib.Extensions;

public static class EnumerableExtensions
{
	public static Option<TV> LookUp<TK, TV>(this IDictionary<TK, TV> input, TK key)
	{
		return input.TryGetValue(key, out TV? val) ? val : Of.None;
	}

	public static Option<T> TryFirst<T>(this IEnumerable<T> enumerable)
	{
		var firstOrNull = enumerable.FirstOrDefault();
		return firstOrNull==null? Of.None:firstOrNull;
	}

	public static Option<T> TryFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
	{
		var filtered = enumerable.FirstOrDefault(predicate);
		return filtered == null ? Of.None: filtered;
	}
}