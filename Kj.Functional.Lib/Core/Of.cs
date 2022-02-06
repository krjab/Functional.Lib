namespace Kj.Functional.Lib.Core;

public static class Of
{
	public static Option.None None
		=> Option.None.Default;
		
	public static Option.Some<T> Some<T>(T value)
		=> new Option.Some<T>(value);
}