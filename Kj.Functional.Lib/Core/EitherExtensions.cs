using JetBrains.Annotations;

namespace Kj.Functional.Lib.Core;

public static class EitherExtensions
{
	/// <summary>
	/// Maps the left side result (if present) to <see cref="TMapped"/>
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapLeft">Map function</param>
	/// <typeparam name="TL">left result type</typeparam>
	/// <typeparam name="TR">right result type</typeparam>
	/// <typeparam name="TMapped">target type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TL,TR}"/></returns>
	[MustUseReturnValue]
	public static Either<TMapped, TR> MapLeft<TL, TR, TMapped>(this Either<TL, TR> either, Func<TL, TMapped> mapLeft)
	{
		return either
			.Match(lv => mapLeft(lv),
				Either<TMapped, TR>.Right<TMapped,TR>);
	}

	[MustUseReturnValue]
	public static Task<Either<TMapped, TError>> MapResultAsync<TResult, TError, TMapped>(this Either<TResult, TError> either,
		Func<TResult, Task<TMapped>> mapLeft)
	{
		return either.MapLeftAsync(mapLeft);
	}
	
	/// <summary>
	/// Maps the left part to another task
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapLeft">Map function</param>
	/// <typeparam name="TL">left result type</typeparam>
	/// <typeparam name="TR">right result type</typeparam>
	/// <typeparam name="TMapped">target type</typeparam>
	/// <returns>tas returning <see cref="Either{TL,TR}"/></returns>
	public static Task<Either<TMapped, TR>> MapLeftAsync<TL, TR, TMapped>(this Either<TL, TR> either, Func<TL, Task<TMapped>> mapLeft)
	{
		return either
			.Match(lv => mapLeft(lv).ToEitherTask<TMapped,TR>(),
				rv => Task.FromResult(Either<TMapped, TR>.Right<TMapped, TR>(rv)));
	}
	
	private static async Task<Either<TL, TR>> ToEitherTask<TL,TR>(this Task<TL> mapTask)
	{
		var res = await mapTask;
		return Either<TL,TR>.Left<TL, TR>(res);
	}

	[MustUseReturnValue]
	public static async Task<Either<TMapped, TR>> MapLeftAsync<TL, TR, TMapped>(this Task<Either<TL, TR>> thisTask,
		Func<TL, Task<TMapped>> mapLeft)
	{
		var res = await thisTask;
		return await res.MapLeftAsync(mapLeft);
	}

	[MustUseReturnValue]
	public static Either<TL, TMapped> MapRight<TL, TR, TMapped>(this Either<TL, TR> either, Func<TR, TMapped> mapRight)
	{
		return either
			.Match(Either<TL, TMapped>.Left<TL, TMapped>,
				r => mapRight(r));
	}

	/// <summary>
	/// Maps the result part to <see cref="TMapped"/>
	/// </summary>
	/// <param name="either">Structure to map</param>
	/// <param name="mapFunc">Map function</param>
	/// <typeparam name="TResult">result type</typeparam>
	/// <typeparam name="TError">error type</typeparam>
	/// <typeparam name="TMapped">target type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TL,TR}"/></returns>
	public static Either<TMapped, TError> MapResult<TResult, TError, TMapped>(this Either<TResult, TError> either,
		Func<TResult, TMapped> mapFunc)
	{
		return either.MapLeft(mapFunc);
	}
	
	[MustUseReturnValue]
	public static Either<TResult, TMapped> MapError<TResult, TError, TMapped>(this Either<TResult, TError> either, Func<TError, TMapped> mapRight)
	{
		return either.MapRight(mapRight);
	}

	[MustUseReturnValue]
	public static Either<TMapped, TR> BindLeft<TL, TR, TMapped>(this Either<TL, TR> either,
		Func<TL, Either<TMapped, TR>> mapLeft)
	{
		return  either
			.Match(mapLeft,
				Either<TMapped, TR>.Right<TMapped, TR>);
	}
	
	[MustUseReturnValue]
	public static Task<Either<TMapped, TR>> BindLeftAsync<TL, TR, TMapped>(this Either<TL, TR> either,
		Func<TL, Task<Either<TMapped, TR>>> mapLeft)
	{
		return either
			.Match(mapLeft,
				rv => Task.FromResult(Either<TMapped, TR>.Right<TMapped, TR>(rv)));
		
	}

	[MustUseReturnValue]
	public static async Task<TMapped> MatchAsync<TL, TR, TMapped>(this Task<Either<TL, TR>> thisTask,
		Func<TL, TMapped> left, Func<TR, TMapped> right)
	{
		var res = await thisTask;

		return  res
			.Match(left,
				right);
		
	}
	
	[MustUseReturnValue]
	public static async Task<TMapped> MatchAsync<TL, TR, TMapped>(this Task<Either<TL, TR>> thisTask,
		Func<TL, Task<TMapped>> left, Func<TR, TMapped> right)
	{
		var res = await thisTask;

		return await res
			.Match(left,
				rv => Task.FromResult(right(rv)));
		
	}

	public static Unit Do<TL, TR>(this Either<TL, TR> either, Action<TL> doLeft, Action<TR> doRight)
	{
		return either.Match(
			vl => doLeft.ToFunc()(vl),
			vr => doRight.ToFunc()(vr)
		);
	}

}