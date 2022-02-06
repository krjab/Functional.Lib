using FsCheck;
using Kj.Functional.Lib.Core;

namespace Kj.Functional.Lib.Test.Core;

internal static class ArbitraryOption
{
	public static Arbitrary<Option<T>> Option<T>()
	{
		var gen1 = Arb.Generate<bool>()
			.SelectMany(isSome => Arb.Generate<T>().Select(v => (isSome, v)))
			.Select(x =>
			{
				(bool isSome, var v) = x;

				Option<T> r = Of.None;
				if (isSome && v != null)
				{
					r =  Of.Some(v);
				}

				return r;
			});
		return gen1.ToArbitrary();
	}
}