using JetBrains.Annotations;

namespace Kj.Functional.Lib.Core;

public static class FuncExtensions
{
	/// <summary>
	/// Converts an action to a Unit returning function.
	/// </summary>
	/// <param name="action">Action to convert</param>
	/// <returns>Unit returning function</returns>
	[MustUseReturnValue]
	public static Func<Unit> ToFunc(this Action action)
	{
		return () =>
		{
			action();
			return Unit.Default;
		};
	}
	
	/// <summary>
	/// Converts an action to a Unit returning function.
	/// </summary>
	/// <param name="action">Action to convert</param>
	/// <returns>Unit returning function</returns>
	[MustUseReturnValue]
	public static Func<T,Unit> ToFunc<T>(this Action<T> action)
	{
		return t =>
		{
			action(t);
			return Unit.Default;
		};
	}
}