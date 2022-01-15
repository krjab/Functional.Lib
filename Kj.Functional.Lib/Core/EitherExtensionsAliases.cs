namespace Kj.Functional.Lib.Core;

public static class EitherExtensionsAliases
{
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
	
	/// <summary>
	/// Maps the success result (if present) to <see cref="TMapped"/>
	/// </summary>
	/// <param name="either">Either structure to map</param>
	/// <param name="mapLeft">Map function</param>
	/// <typeparam name="TMapped">success result type</typeparam>
	/// <typeparam name="TResult">error result type</typeparam>
	/// <returns>mapped instance of <see cref="Either{TL,TR}"/></returns>
	public static Task<Either<TMapped, TError>> MapResultAsync<TResult, TError, TMapped>(this Either<TResult, TError> either,
		Func<TResult, Task<TMapped>> mapLeft)
	{
		return either.MapLeftAsync(mapLeft);
	}
	
		
	public static Either<TResult, TMapped> MapError<TResult, TError, TMapped>(this Either<TResult, TError> either, Func<TError, TMapped> mapRight)
	{
		return either.MapRight(mapRight);
	}
	
	public static Task<Either<TMapped, TError>> BindResultAsync<TResult, TError, TMapped>(this Either<TResult, TError> either,
		Func<TResult, Task<Either<TMapped, TError>>> mapResult)
	{
		return either.BindLeftAsync(mapResult);
	}
	
	public static async Task<Either<TMapped, TError>> BindResultAsync<TResult, TError, TMapped>(this Task<Either<TResult, TError>> eitherTask,
		Func<TResult, Task<Either<TMapped, TError>>> mapResult)
	{
		var takResult = await eitherTask;
		return await takResult
			.BindResultAsync(mapResult);
	}
}