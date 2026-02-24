namespace Kj.Functional.Lib.Core;

/// <summary>
/// Unit type, meant as a result type for functions returning nothing.
/// </summary>
public readonly struct Unit : IEquatable<Unit>
{
	public static Unit Default => new Unit();

    public override bool Equals(object? obj)
    {
        return obj is Unit;
    }

    public override int GetHashCode()
    {
        return Default.GetHashCode();
    }

    public static bool operator ==(Unit left, Unit right)
    {
        return true;
    }

    public static bool operator !=(Unit left, Unit right)
    {
        return !(left == right);
    }

    public bool Equals(Unit other)
    {
        return true;
    }
}