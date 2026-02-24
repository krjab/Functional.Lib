using System.Diagnostics.CodeAnalysis;

namespace Option;

[SuppressMessage("Performance", "CA1805:Do not initialize unnecessarily")]
public readonly struct None : IEquatable<None>
{
	internal static readonly None Default = new None();

    public override bool Equals(object? obj)
    {
        return obj is None;
    }

    public override int GetHashCode()
    {
        return Default.GetHashCode();
    }

    public static bool operator ==(None left, None right)
    {
        return true;
    }

    public static bool operator !=(None left, None right)
    {
        return !(left == right);
    }

    public bool Equals(None other)
    {
        return true;
    }
}