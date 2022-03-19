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

	/// <summary>
	/// "Reduces" the argument func of 1 parameter to the func of 0 parameters, applying first parameter. 
	/// </summary>
	/// <param name="thisFunc">Function to apply for</param>
	/// <param name="t1">Parameter to apply</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <returns>Function T1->TR</returns>
	public static Func<TR> Apply<T1, TR>(this Func<T1, TR> thisFunc, T1 t1)
		=> () => thisFunc(t1);
	
	/// <summary>
	/// "Reduces" the argument func of 2 parameters to the func of 1 parameter, applying first parameter. 
	/// </summary>
	/// <param name="thisFunc">Function to apply for</param>
	/// <param name="t1">Parameter to apply</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <returns>Function T2->TR</returns>
	public static Func<T2, TR> Apply<T1, T2, TR>(this Func<T1, T2, TR> thisFunc, T1 t1)
		=> t2 => thisFunc(t1, t2);

	/// <summary>
	/// "Reduces" the argument func of 3 parameters to the func of 2 parameters, applying first parameter. 
	/// </summary>
	/// <param name="thisFunc">Function to apply for</param>
	/// <param name="t1">Parameter to apply</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <returns>Function (T2,T3)->TR</returns>
	public static Func<T2, T3, TR> Apply<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> thisFunc, T1 t1)
		=> (t2, t3) => thisFunc(t1, t2, t3);
	
	
	/// <summary>
	/// Curries to Func T1 -> (T2 -> TR)
	/// </summary>
	/// <param name="func">input func</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <returns>Curried input func</returns>
	public static Func<T1, Func<T2, TR>> Curry<T1, T2, TR>(this Func<T1, T2, TR> func)
		=> t1 => t2 => func(t1, t2);
	
	/// <summary>
	/// Curries to Func T1 -> (T2 -> (T3->TR))
	/// </summary>
	/// <param name="func"></param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <typeparam name="TR"></typeparam>
	/// <returns></returns>
	public static Func<T1, Func<T2, Func<T3, TR>>> Curry<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> func)
		=> t1 => t2 => t3 => func(t1, t2, t3);


	/// <summary>
	/// Composes <paramref name="func1"/> with <paramref name="another"/> to create 1 function from <typeparamref name="T1"/> to <typeparamref name="T3"/>
	/// </summary>
	/// <param name="func1">Input function</param>
	/// <param name="another">Function to compose with</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <returns>Composed function</returns>
	public static Func<T1, T3> ComposeWith<T1, T2, T3>(this Func<T1, T2> func1, Func<T2, T3> another)
	{
		return x => another(func1(x));
	}

	/// <summary>
	/// Composes task returning <paramref name="func1"/> with <paramref name="another"/> to create 1 function from <typeparamref name="T1"/> to task returning <typeparamref name="T3"/>
	/// </summary>
	/// <param name="func1">Input function</param>
	/// <param name="another">Function to compose with</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <returns>Composed function</returns>
	public static Func<T1, Task<T3>> ComposeWith<T1, T2, T3>(this Func<T1, Task<T2>> func1, Func<T2, T3> another)
	{
		return async t1 =>
		{
			var v2 = await func1(t1);
			return another(v2);
		};
	}
	
	/// <summary>
	/// Composes task returning <paramref name="func1"/> with task returning <paramref name="another"/> to create 1 function from <typeparamref name="T1"/> to task returning <typeparamref name="T3"/>
	/// </summary>
	/// <param name="func1">Input function</param>
	/// <param name="another">Function to compose with</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <returns>Composed function</returns>
	public static Func<T1, Task<T3>> ComposeWith<T1, T2, T3>(this Func<T1, Task<T2>> func1, Func<T2, Task<T3>> another)
	{
		return async t1 =>
		{
			var v1 = await func1(t1);
			return await another(v1);
		};
	}
	
	/// <summary>
	/// Composes option returning <paramref name="func1"/> with <paramref name="another"/> to create 1 function from <typeparamref name="T1"/> to optional <typeparamref name="T3"/> 
	/// </summary>
	/// <param name="func1">Input function returning option</param>
	/// <param name="another">Function to compose with</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <returns>Composed function</returns>
	public static Func<T1, Option<T3>> ComposeWith<T1, T2, T3>(this Func<T1, Option<T2>> func1, Func<T2, T3> another)
	{
		Func<T1, Option<T3>> fComp = x => func1(x).Map(another);
		return fComp;
	}

	/// <summary>
	/// Composes task of option returning <paramref name="func1"/> with <paramref name="another"/> to create 1 function from <typeparamref name="T1"/> to optional <typeparamref name="T3"/> 
	/// </summary>
	/// <param name="func1">Input function returning option</param>
	/// <param name="another">Function to compose with</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <returns>Composed function</returns>
	public static Func<T1, Task<Option<T3>>> ComposeWith<T1, T2, T3>(this Func<T1, Task<Option<T2>>> func1, Func<T2, T3> another)
	{
		return async t1 =>
		{
			var v2 = await func1(t1);
			return v2.Map(another);
		};
	}

	/// <summary>
	/// Composes Either returning <paramref name="func1"/> with <paramref name="another"/> to create 1 function from <typeparamref name="T1"/> to either of <typeparamref name="T3"/>/<typeparamref name="TError"/> 
	/// </summary>
	/// <param name="func1">Input function returning option</param>
	/// <param name="another">Function to compose with</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <typeparam name="TError"></typeparam>
	/// <returns>Composed function</returns>
	public static Func<T1, Either<T3, TError>> ComposeWith<T1, T2, TError, T3>(this Func<T1, Either<T2,TError>> func1, Func<T2, T3> another)
	{
		Func<T1, Either<T3, TError>> fComp = x => func1(x).MapLeft(another);
		return fComp;
	}
	
	/// <summary>
	/// Composes task of Either returning <paramref name="func1"/> with <paramref name="another"/> to create 1 function from <typeparamref name="T1"/> to either of <typeparamref name="T3"/>/<typeparamref name="TError"/> 
	/// </summary>
	/// <param name="func1">Input function returning option</param>
	/// <param name="another">Function to compose with</param>
	/// <typeparam name="T1"></typeparam>
	/// <typeparam name="T2"></typeparam>
	/// <typeparam name="T3"></typeparam>
	/// <typeparam name="TError"></typeparam>
	/// <returns>Composed function</returns>
	public static Func<T1, Task<Either<T3, TError>>> ComposeWith<T1, T2, TError, T3>(this Func<T1, Task<Either<T2,TError>>> func1, Func<T2, T3> another)
	{
		return async t1 =>
		{
			var v2 = await func1(t1);
			return v2.MapResult(another);
		};
	}

	/// <summary>
	/// Calls the given function, catching an exception thrown and returning either <typeparamref name="T"/> or an exception.
	/// </summary>
	/// <param name="func">Func to call</param>
	/// <typeparam name="T"></typeparam>
	/// <returns>Either T or exception</returns>
	public static Either<T, Exception> TryInvoke<T>(this Func<T> func)
	{
		try
		{
			return func();
		}
		catch (Exception e)
		{
			return Either<T, Exception>.Right(e);
		}
	}

}