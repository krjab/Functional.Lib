using JetBrains.Annotations;

namespace Kj.Functional.Lib.Core;

public static class FuncExtensions
{
	[MustUseReturnValue]
	public static Func<Unit> ToFunc(this Action action)
	{
		return () =>
		{
			action();
			return Unit.Default;
		};
	}
	
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