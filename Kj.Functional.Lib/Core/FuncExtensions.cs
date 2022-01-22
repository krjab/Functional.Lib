namespace Kj.Functional.Lib.Core;

public static class FuncExtensions
{
	/// <summary>
	/// Converts an action to a Unit returning function.
	/// </summary>
	/// <param name="action">Action to convert</param>
	/// <returns>Unit returning function</returns>
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
	public static Func<T,Unit> ToFunc<T>(this Action<T> action)
	{
		return t =>
		{
			action(t);
			return Unit.Default;
		};
	}
	
	
	internal static async Task<T> DoTaskAndReturn<T>(this Func<Task> rightResultTaskFunc,
		T toReturn)
	{
		await rightResultTaskFunc();
		return toReturn;
	}
	
	// TODO - Documentation
	public static Func<T2, TR> Apply<T1, T2, TR>(this Func<T1, T2, TR> f, T1 t1)
		=> t2 => f(t1, t2);

	// TODO - Documentation
	public static Func<T2, T3, TR> Apply<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> f, T1 t1)
		=> (t2, t3) => f(t1, t2, t3);
	
	
	// TODO - Documentation
	public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(this Func<T1, T2, TR> func)
		=> t1 => t2 => func(t1, t2);
	
	// TODO - Documentation
	public static Func<T1, Func<T2, Func<T3, TR>>> Curry<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func)
		=> t1 => t2 => t3 => func(t1, t2, t3);
	
}