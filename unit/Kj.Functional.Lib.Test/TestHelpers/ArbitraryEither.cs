using FsCheck;
using Kj.Functional.Lib.Core;

namespace Kj.Functional.Lib.Test.TestHelpers;

internal static class ArbitraryEither
{
	public static Arbitrary<Either<TL,TR>> Either<TL,TR>()
	{
		return Arb.Generate<bool>()
			.SelectMany(isLeft =>
			{
				if (isLeft)
				{
					return Arb.Generate<TL>()
						.Select(Lib.Core.Either<TL, TR>.Left);
				}

				return Arb.Generate<TR>()
					.Select(Lib.Core.Either<TL, TR>.Right);
			})
			.ToArbitrary();
	}
}