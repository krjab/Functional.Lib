using AutoFixture;
using AutoFixture.Kernel;

namespace Kj.Functional.Lib.Test.TestHelpers.Fixture;

public static class AutofixtureExtensions
{
	public static int CreateInt(this ISpecimenBuilder builder, int min, int max)
	{
		return builder.Create<int>() % (max - min + 1) + min;
	}
}