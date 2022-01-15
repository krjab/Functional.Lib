using JetBrains.Annotations;
using Option;

namespace Kj.Functional.Lib.Core;

public static class OptionExtensions
{
	/// <summary>
	/// Maps the inner value (if present) to another value and returns an Option.
	/// </summary>
	/// <param name="optT">option to use</param>
	/// <param name="f">mapping function</param>
	/// <typeparam name="TS">input option type</typeparam>
	/// <typeparam name="TR">mapped option type</typeparam>
	/// <returns>Option (Some with mapped value or None)</returns>
	[MustUseReturnValue]
	public static Option<TR> Map<TS, TR>(this Option<TS> optT, Func<TS, TR> f)
		=> optT.Match<Option<TR>>(
			t => Of.Some(f(t)),
			() => Of.None
		);

	/// <summary>
	/// Performs a side effect causing action, either on underlying value or another action for none.
	/// </summary>
	/// <param name="option">Option to use</param>
	/// <param name="some">Action to apply if underlying value present.</param>
	/// <param name="none">Action to apply if no value present</param>
	/// <typeparam name="T">Underlying value type.</typeparam>
	/// <returns>This Option instance</returns>
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

	/// <summary>
	/// Performs a side effect causing action on underlying value (if present).
	/// </summary>
	/// <param name="opt">Option to use</param>
	/// <param name="action">Action to perform</param>
	/// <typeparam name="T">Underlying type</typeparam>
	/// <returns>This Option instance</returns>
	public static Option<T> ForEach<T>(this Option<T> opt, Action<T> action)
		=> Bind(opt, v =>
		{
			action(v);
			return opt;
		});
	
	/// <summary>
	/// Performs a side effect causing task returning function on underlying value (if present).
	/// </summary>
	/// <param name="opt">Option to use</param>
	/// <param name="action">Action to perform</param>
	/// <typeparam name="T">Underlying type</typeparam>
	/// <returns>This Option instance</returns>
	public static Task<Option<T>> ForEachAsync<T>(this Option<T> opt, Func<T,Task> taskFunc)
		=> BindAsync(opt, v =>
		{
			var f = () => taskFunc(v);
			return f.DoTaskAndReturn(opt);
		});
	

	/// <summary>
	/// Uses the underlying value (if present) to use by another Option returning function. 
	/// </summary>
	/// <param name="optT">Option to use</param>
	/// <param name="f">Function consuming the underlying value.</param>
	/// <typeparam name="T">Source underlying type.</typeparam>
	/// <typeparam name="TMapped">Target underlying type</typeparam>
	/// <returns>Some of mapped value or none</returns>
	[MustUseReturnValue]
	public static Option<TMapped> Bind<T, TMapped>(this Option<T> optT, Func<T, Option<TMapped>> f)
		=> optT.Match(
			f,
			() => Of.None
		);

	/// <summary>
	/// Uses the underlying value (if present) to use by another Option task returning function. 
	/// </summary>
	/// <param name="optT">Option to use</param>
	/// <param name="taskFunc">Function consuming the underlying value as task</param>
	/// <typeparam name="T">Source underlying type.</typeparam>
	/// <typeparam name="TMapped">Target underlying type</typeparam>
	/// <returns>Task containing some of mapped value or none</returns>
	public static Task<Option<TMapped>> BindAsync<T, TMapped>(this Option<T> optT,
		Func<T, Task<Option<TMapped>>> taskFunc)
	{
		Option<TMapped> n = Of.None;
		return optT
			.Match(taskFunc,
				() => Task.FromResult(n));
	}

	/// <summary>
	/// Uses the underlying value (if present) to use by an Either returning function. 
	/// </summary>
	/// <param name="option">Option object</param>
	/// <param name="bindFunc">Mapping function to consume the option's value (if present)</param>
	/// <param name="createRightFunc">Function creating right side value (in case mapping from None)</param>
	/// <typeparam name="TSource">Source underlying type</typeparam>
	/// <typeparam name="TR">Right side type</typeparam>
	/// <typeparam name="TMapped">Mapped target type</typeparam>
	/// <returns>Instance ot Either(TMapped, TR)</returns>
	[MustUseReturnValue]
	public static Either<TMapped, TR> Bind<TSource, TR, TMapped>(this Option<TSource> option,
		Func<TSource, Either<TMapped, TR>> bindFunc,
		Func<TR> createRightFunc)
	{
		return option
			.Match(bindFunc,
				() => Either<TMapped, TR>.Right(createRightFunc()));
	}
	
	/// <summary>
	/// Extracts the present underlying values  from the elements within  the collection
	/// </summary>
	/// <param name="input">input collection</param>
	/// <typeparam name="T">Underlying type</typeparam>
	/// <returns>Collection of extracted (present) values</returns>
	[MustUseReturnValue]
	public static IEnumerable<T> Bind<T>(this IEnumerable<Option<T>> input)
	{
		return input.SelectMany(t => t);
	}

	/// <summary>
	/// If the underlying value is present AND satisfies the specified condition then the same option object is returned; otherwise None.
	/// </summary>
	/// <param name="option">Input Option</param>
	/// <param name="predicate">Condition to check</param>
	/// <typeparam name="T">Underlying type</typeparam>
	/// <returns>Some if value present and satisfies condition, None if otherwise.</returns>
	[MustUseReturnValue]
	public static Option<T> Where<T>(this Option<T> option, Func<T, bool> predicate)
	{
		return option
			.Match(v => predicate(v) ? option : Of.None, () => Of.None);
	}

}