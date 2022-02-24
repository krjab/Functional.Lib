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
	/// Uses the success result (if present) as parameter to another Either returning function and returns
	/// this next function's result.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="bindResultFunc">Function to use the left side value</param>
	/// <typeparam name="TL">Left side value type</typeparam>
	/// <typeparam name="TR">Right side value type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TMapped,TR}"/></returns>
	public static Either<TMapped, TR> BindResult<TL, TR, TMapped>(this Either<TL, TR> either, Func<TL, Either<TMapped, TR>> bindResultFunc)
	{
		return either.BindLeft(bindResultFunc);
	}
	
	/// <summary>
	/// Uses the success result (if present) as parameter to another Task of Either returning function and returns
	/// this next function's result.
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapLeft">Function to use the left side value</param>
	/// <typeparam name="TL">Left side value type</typeparam>
	/// <typeparam name="TR">Right side value type</typeparam>
	/// <typeparam name="TMapped">Mapped type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TMapped,TR}"/></returns>
	public static Task<Either<TMapped, TR>> BindResultAsync<TL, TR, TMapped>(this Either<TL, TR> either,
		Func<TL, Task<Either<TMapped, TR>>> mapLeft)
	{
		return either.BindLeftAsync(mapLeft);
		
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
	/// Maps the error part to TMapped returning task.
	/// </summary>
	/// <param name="either">Structure to map</param>
	/// <param name="mapRight">Error Map function</param>
	/// <typeparam name="TResult">result type</typeparam>
	/// <typeparam name="TError">error type</typeparam>
	/// <typeparam name="TMapped">target type</typeparam>
	/// <returns>task returning Either(TResult, TMapped)/></returns>
	public static Task<Either<TResult, TMapped>> MapErrorAsync<TResult, TError, TMapped>(this Either<TResult, TError> either, Func<TError, Task<TMapped>> mapRight)
	{
		return either.MapRightAsync(mapRight);
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
	
	/// <summary>
	/// Performs a side-effect action for result (if present)
	/// </summary>
	/// <param name="either">input Either</param>
	/// <param name="resultAction">action to perform</param>
	/// <typeparam name="TL"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <returns>Original structure</returns>
	public static Either<TL, TR> DoWithResult<TL, TR>(this Either<TL, TR> either, Action<TL> resultAction)
	{
		return either
			.DoWithLeft(resultAction);
	}
	
	/// <summary>
	/// Performs a side-effect action for error (if present)
	/// </summary>
	/// <param name="either">input Either</param>
	/// <param name="errorAction">action to perform</param>
	/// <typeparam name="TL"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <returns>Original structure</returns>
	public static Either<TL, TR> DoWithError<TL, TR>(this Either<TL, TR> either, Action<TR> errorAction)
	{
		return either
			.DoWithRight(errorAction);
	}

	/// <summary>
	/// Converts the either into an option, taking the left side (success) value as Some
	/// or discarding the right (error) side as None.
	/// </summary>
	/// <param name="thisEither">this Either</param>
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TError"></typeparam>
	/// <returns>Option of <typeparamref name="TResult"/> </returns>
	public static Option<TResult> AsOption<TResult, TError>(this Either<TResult, TError> thisEither)
	{
		return thisEither
			.Match<Option<TResult>>(v => v, _ => Of.None);
	}
}