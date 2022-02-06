namespace Kj.Functional.Lib.Core;

public static class EitherExtensions
{
	/// <summary>
	/// Maps the left side result (if present) to TMapped
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapLeft">Map function</param>
	/// <typeparam name="TL">left result type</typeparam>
	/// <typeparam name="TR">right result type</typeparam>
	/// <typeparam name="TMapped">target type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TL,TR}"/></returns>
	public static Either<TMapped, TR> MapLeft<TL, TR, TMapped>(this Either<TL, TR> either, Func<TL, TMapped> mapLeft)
	{
		return either
			.Match(lv => mapLeft(lv),
				Either<TMapped, TR>.Right);
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
				rv => Task.FromResult(Either<TMapped, TR>.Right(rv)));
	}
	
	private static async Task<Either<TL, TR>> ToEitherTask<TL,TR>(this Task<TL> mapTask)
	{
		var res = await mapTask;
		return Either<TL,TR>.Left(res);
	}
	


	/// <summary>
	/// Awaits the task and maps the result further (<see cref="MapLeftAsync{TL,TR,TMapped}(Kj.Functional.Lib.Core.Either{TL,TR},System.Func{TL,System.Threading.Tasks.Task{TMapped}})"/>
	/// </summary>
	/// <param name="thisTask"></param>
	/// <param name="mapLeft"></param>
	/// <typeparam name="TL"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <typeparam name="TMapped"></typeparam>
	/// <returns></returns>
	public static async Task<Either<TMapped, TR>> MapLeftAsync<TL, TR, TMapped>(this Task<Either<TL, TR>> thisTask,
		Func<TL, Task<TMapped>> mapLeft)
	{
		var res = await thisTask;
		return await res.MapLeftAsync(mapLeft);
	}

	/// <summary>
	/// Maps the right side result (if present) to TMapped.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapRight">Map function</param>
	/// <typeparam name="TL">left result type</typeparam>
	/// <typeparam name="TR">right result type</typeparam>
	/// <typeparam name="TMapped">target type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TL,TR}"/></returns>
	public static Either<TL, TMapped> MapRight<TL, TR, TMapped>(this Either<TL, TR> either, Func<TR, TMapped> mapRight)
	{
		return either
			.Match(Either<TL, TMapped>.Left,
				r => mapRight(r));
	}

	/// <summary>
	/// Maps the right side result (if present) to task returning TMapped.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapRight">Map function</param>
	/// <typeparam name="TL">left result type</typeparam>
	/// <typeparam name="TR">right result type</typeparam>
	/// <typeparam name="TMapped">target type</typeparam>
	/// <returns>task returning <see cref="Either{TL,TR}"/></returns>
	public static Task<Either<TL, TMapped>> MapRightAsync<TL, TR, TMapped>(this Either<TL, TR> either, Func<TR, Task<TMapped>> mapRight)
	{
		return  either
			.Match(
				rv => Task.FromResult(Either<TL, TMapped>.Left(rv)),
				lv => mapRight(lv).ToEitherTask<TL,TMapped>());
	}

	private static async Task<Either<TL, TR>> ToEitherTask<TL,TR>(this Task<TR> mapTask)
	{
		var res = await mapTask;
		return Either<TL,TR>.Right(res);
	}
	
	/// <summary>
	/// Uses the left side result (if present) as parameter to another Either returning function and returns
	/// this next function's result.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapLeft">Function to use the left side value</param>
	/// <typeparam name="TL">Left side value type</typeparam>
	/// <typeparam name="TR">Right side value type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TMapped,TR}"/></returns>
	public static Either<TMapped, TR> BindLeft<TL, TR, TMapped>(this Either<TL, TR> either,
		Func<TL, Either<TMapped, TR>> mapLeft)
	{
		return  either
			.Match(mapLeft,
				Either<TMapped, TR>.Right);
	}
	
	/// <summary>
	/// Uses the left side result (if present) as parameter to another Task of Either returning function and returns
	/// this next function's result.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapLeft">Function to use the left side value</param>
	/// <typeparam name="TL">Left side value type</typeparam>
	/// <typeparam name="TR">Right side value type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TMapped,TR}"/></returns>
	public static Task<Either<TMapped, TR>> BindLeftAsync<TL, TR, TMapped>(this Either<TL, TR> either,
		Func<TL, Task<Either<TMapped, TR>>> mapLeft)
	{
		return either
			.Match(mapLeft,
				rv => Task.FromResult(Either<TMapped, TR>.Right(rv)));
		
	}
	
	/// <summary>
	/// Awaits the task and maps the result further using BindLeftAsync./>
	/// </summary>
	/// <param name="thisTask">Task to await.</param>
	/// <param name="mapLeft">Function using the result of the input task.</param>
	/// <typeparam name="TL">Left side type</typeparam>
	/// <typeparam name="TR">Right side type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>Task returning Either (TMapped, TR) /></returns>
	public static async Task<Either<TMapped, TR>> BindLeftAsync<TL, TR, TMapped>(this Task<Either<TL, TR>> thisTask,
		Func<TL, Task<Either<TMapped, TR>>> mapLeft)
	{
		var takResult = await thisTask;
		return await takResult
			.BindLeftAsync(mapLeft);
	}
	
	/// <summary>
	/// Awaits the task, extracts the left or right side value and maps it to the TMapped.
	/// </summary>
	/// <param name="thisTask">Task to await</param>
	/// <param name="left">left side value mapping function</param>
	/// <param name="right">right side value mapping function</param>
	/// <typeparam name="TL">left side type</typeparam>
	/// <typeparam name="TR">right side type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>Task returning TMapped</returns>
	public static async Task<TMapped> MatchAsync<TL, TR, TMapped>(this Task<Either<TL, TR>> thisTask,
		Func<TL, TMapped> left, Func<TR, TMapped> right)
	{
		var res = await thisTask;

		return  res
			.Match(left,
				right);
		
	}
	
	/// <summary>
	/// Awaits the task, extracts the left or right side value and maps it to the TMapped.
	/// </summary>
	/// <param name="thisTask">Task to await</param>
	/// <param name="left">left side value to task mapping function</param>
	/// <param name="right">right side value mapping function</param>
	/// <typeparam name="TL">left side type</typeparam>
	/// <typeparam name="TR">right side type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>Task returning TMapped</returns>
	public static async Task<TMapped> MatchAsync<TL, TR, TMapped>(this Task<Either<TL, TR>> thisTask,
		Func<TL, Task<TMapped>> left, Func<TR, TMapped> right)
	{
		var res = await thisTask;

		return await res
			.Match(left,
				rv => Task.FromResult(right(rv)));
		
	}

	/// <summary>
	/// Performs a side effect causing action on left or right side value.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="leftResultAction">action to perform on the left side value</param>
	/// <param name="rightResultAction">action to perform on the right side value</param>
	/// <typeparam name="TL">Left side type</typeparam>
	/// <typeparam name="TR">Right side type</typeparam>
	/// <returns>Original structure</returns>
	public static Either<TL, TR> Do<TL, TR>(this Either<TL, TR> either, Action<TL> leftResultAction,
		Action<TR> rightResultAction)
	{
		return either
			.Match(
				lv =>
				{
					leftResultAction(lv);
					return either;
				},
				rv =>
				{
					rightResultAction(rv);
					return either;
				}
			);
	}
	
	/// <summary>
	/// Performs a side effect causing action on left side value.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="leftResultAction">action to perform on the left side value</param>
	/// <typeparam name="TL">Left side type</typeparam>
	/// <typeparam name="TR">Right side type</typeparam>
	/// <returns>Original structure</returns>
	public static Either<TL, TR> DoWithLeft<TL, TR>(this Either<TL, TR> either, Action<TL> leftResultAction)
	{
		return either
			.Do(leftResultAction, _ => { });
	}

	/// <summary>
	/// Performs a side effect causing action on right side value.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="rightResultAction">action to perform on the righ side value</param>
	/// <typeparam name="TL">Left side type</typeparam>
	/// <typeparam name="TR">Right side type</typeparam>
	/// <returns>Original structure</returns>
	public static Either<TL, TR> DoWithRight<TL, TR>(this Either<TL, TR> either, Action<TR> rightResultAction)
	{
		return either
			.Do(_ => { },
				rightResultAction);
	}
	
	/// <summary>
	/// Performs a side effect causing action on left or right side value.
	/// </summary>
	/// <param name="thisEither">Either structure to map</param>
	/// <param name="leftResultAction">action to perform on the left side value</param>
	/// <param name="rightResultTaskFunc">task returning function to perform on the right side value</param>
	/// <typeparam name="TL">Left side type</typeparam>
	/// <typeparam name="TR">Right side type</typeparam>
	/// <returns>Original structure</returns>
	public static Task<Either<TL, TR>> DoAsync<TL, TR>(this Either<TL, TR> thisEither, Action<TL> leftResultAction,
		Func<TR, Task> rightResultTaskFunc)
	{
		return thisEither
			.Match(lv =>
				{
					leftResultAction(lv);
					return Task.FromResult(thisEither);
				},
				rv =>
				{
					var taskFunc = () => rightResultTaskFunc(rv);
					return taskFunc.DoTaskAndReturn(thisEither);
				}
			);
	}

	/// <summary>
	/// Performs a side effect causing action on left or right side value.
	/// </summary>
	/// <param name="thisEither">Either structure to map</param>
	/// <param name="leftResultTaskFunc">task returning function to perform on the left side value</param>
	/// <param name="rightResultAction">action to perform on the right side value</param>
	/// <typeparam name="TL">Left side type</typeparam>
	/// <typeparam name="TR">Right side type</typeparam>
	/// <returns>Original structure</returns>
	public static Task<Either<TL, TR>> DoAsync<TL, TR>(this Either<TL, TR> thisEither, Func<TL, Task> leftResultTaskFunc,
		Action<TR> rightResultAction)
	{
		return thisEither
			.Match(lv =>
				{
					var taskFunc = () => leftResultTaskFunc(lv);
					return taskFunc.DoTaskAndReturn(thisEither);
				},
				rv =>
				{
					rightResultAction(rv);
					return Task.FromResult(thisEither);
				}
			);
	}
	
	/// <summary>
	/// Performs a side effect causing action on left or right side value.
	/// </summary>
	/// <param name="thisEither">Either structure to map</param>
	/// <param name="leftResultTaskFunc">task returning function to perform on the left side value</param>
	/// <param name="rightResultTaskFunc">task returning function to perform on the right side value</param>
	/// <typeparam name="TL">Left side type</typeparam>
	/// <typeparam name="TR">Right side type</typeparam>
	/// <returns>Original structure</returns>
	public static Task<Either<TL, TR>> DoAsync<TL, TR>(this Either<TL, TR> thisEither, Func<TL, Task> leftResultTaskFunc,
		Func<TR, Task> rightResultTaskFunc)
	{
		return thisEither
			.Match(lv =>
				{
					var taskFunc = () => leftResultTaskFunc(lv);
					return taskFunc.DoTaskAndReturn(thisEither);
				},
				rv =>
				{
					var taskFunc = () => rightResultTaskFunc(rv);
					return taskFunc.DoTaskAndReturn(thisEither);;
				}
			);
	}

}