using AutoFixture;
using AutoFixture.Kernel;
using Kj.Functional.Lib.Core;

namespace Kj.Functional.Lib.Test.TestHelpers.Fixture;

public static class AutofixtureExtensions
{
	public static int CreateInt(this ISpecimenBuilder builder, int min, int max)
	{
		return builder.Create<int>() % (max - min + 1) + min;
	}

	public static Either<TL, TR> CreateEither<TL, TR>(this ISpecimenBuilder builder)
	{
		bool isLeft = builder.Create<bool>();
		if (isLeft)
		{
			return builder.Create<TL>();
		}

		return builder.Create<TR>();
	}
}