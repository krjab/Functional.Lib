using JetBrains.Annotations;
using Option;

namespace Kj.Functional.Lib.Core;

public static class OptionExtensions
{
	[MustUseReturnValue]
	public static Option<TR> Map<TS, TR>(this Option<TS> optT, Func<TS, TR> f)
		=> optT.Match<Option<TR>>(
			t => Of.Some(f(t)),
			() => Of.None
		);

	public static Option<T> Do<T>(this Option<T> option, Action<T> some, Action none)
	{
		return option.Match(r =>
			{
				some.ToFunc()(r);
				return option;
			},
			() =>
			{
				none.ToFunc()();
				return option;
			});
	}

	public static Option<Unit> ForEach<T>(this Option<T> opt, Action<T> action)
		=> Map(opt, action.ToFunc());
	
	[MustUseReturnValue]
	public static Option<R> Bind<T, R>(this Option<T> optT, Func<T, Option<R>> f)
		=> optT.Match(
			f,
			() => Of.None
		);

	[MustUseReturnValue]
	public static IEnumerable<T> Bind<T>(this IEnumerable<Option<T>> input)
	{
		return input.SelectMany(t => t);
	}

	[MustUseReturnValue]
	public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate)
	{
		return option
			.Match(v => predicate(v) ? option : Of.None, () => Of.None);
	}

}