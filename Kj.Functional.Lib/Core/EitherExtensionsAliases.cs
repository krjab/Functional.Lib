namespace Kj.Functional.Lib.Core;

public static class EitherExtensionsAliases
{
	/// <summary>
	/// Maps the result part to TMapped
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
	
	/// <summary>
	/// Maps the success result (if present) to task returning TMapped/>
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapLeft">Map function</param>
	/// <typeparam name="TResult">success result type</typeparam>
	/// <typeparam name="TMapped">mapped result type</typeparam>
	/// <typeparam name="TError">error result type</typeparam>
	/// <returns>Task returning instance of Either(TMapped,TError) "/></returns>
	public static Task<Either<TMapped, TError>> MapResultAsync<TResult, TError, TMapped>(this Either<TResult, TError> either,
		Func<TResult, Task<TMapped>> mapLeft)
	{
		return either.MapLeftAsync(mapLeft);
	}
	
		
	/// <summary>
	/// Maps the result part to TMapped
	/// </summary>
	/// <param name="either">Structure to map</param>
	/// <param name="mapRight">Error Map function</param>
	/// <typeparam name="TResult">result type</typeparam>
	/// <typeparam name="TError">error type</typeparam>
	/// <typeparam name="TMapped">target type</typeparam>
	/// <returns>mapped instance of Either(TResult, TMapped)/></returns>
	public static Either<TResult, TMapped> MapError<TResult, TError, TMapped>(this Either<TResult, TError> either, Func<TError, TMapped> mapRight)
	{
		return either.MapRight(mapRight);
	}
	
	/// <summary>
	/// Uses the result (if present) as parameter to another Task of Either returning function and returns
	/// this next function's result.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapResult">Task returning function to use the left side value</param>
	/// <typeparam name="TResult">Left side value type</typeparam>
	/// <typeparam name="TError">Right side value type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>Task returning Either(TMapped, TError)/></returns>
	public static Task<Either<TMapped, TError>> BindResultAsync<TResult, TError, TMapped>(this Either<TResult, TError> either,
		Func<TResult, Task<Either<TMapped, TError>>> mapResult)
	{
		return either.BindLeftAsync(mapResult);
	}
	
	/// <summary>
	/// Awaits the task returning the result (if present) and uses it as parameter to another Task of Either returning function and returns
	/// this next function's result.
	/// </summary>
	/// <param name="eitherTask">Either structure to map</param>
	/// <param name="mapResult">Task returning function to use the left side value</param>
	/// <typeparam name="TResult">Left side value type</typeparam>
	/// <typeparam name="TError">Right side value type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>Task returning Either(TMapped, TError)/></returns>
	public static async Task<Either<TMapped, TError>> BindResultAsync<TResult, TError, TMapped>(this Task<Either<TResult, TError>> eitherTask,
		Func<TResult, Task<Either<TMapped, TError>>> mapResult)
	{
		var takResult = await eitherTask;
		return await takResult
			.BindResultAsync(mapResult);
	}
}