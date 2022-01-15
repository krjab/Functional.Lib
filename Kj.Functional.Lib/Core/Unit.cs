namespace Kj.Functional.Lib.Core;

/// <summary>
/// Unit type, meant as a result type for functions returning nothing.
/// </summary>
public readonly struct Unit
{
	public static Unit Default => new Unit();
}