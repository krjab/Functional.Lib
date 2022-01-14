using AutoFixture;
using FluentAssertions;
using Kj.Functional.Lib.Core;
using NUnit.Framework;

namespace Kj.Functional.Lib.Test.Core;

[TestFixture]
public class EitherTests
{
	private Fixture _fixture = null!;
	
	[SetUp]
	public void Setup()
	{
		_fixture = new Fixture();
	}

	[Test]
	public void Left_Converted()
	{
		int val = _fixture.Create<int>();
		Either<int, string> either = val;

		either.Match(i => i,
			_ =>
			{
				Assert.Fail();
				return -1;
			}).Should().Be(val);
	}
	
	[Test]
	public void Right_Converted()
	{
		int val = _fixture.Create<int>();
		Either<string, int> either = val;

		either.Match(_ =>
				{
					Assert.Fail();
					return -1;
				},
				i => i)
			.Should().Be(val);
	}
}