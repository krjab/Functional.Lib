using AutoFixture;
using FluentAssertions;
using Kj.Functional.Lib.Core;
using NUnit.Framework;

namespace Kj.Functional.Lib.Test.Core;

[TestFixture]
public class OptionToEitherTests
{
	private Fixture _fixture = null!;

	[SetUp]
	public void Setup()
	{
		_fixture = new Fixture();
	}

	[Test]
	public void Option_Some_To_Either()
	{
		int actualValue = _fixture.Create<int>();
		Option<int> option = actualValue;

		var either = option
			.ToEither(() => "invalid, should not be relevant");

		either
			.Do(x => x.Should().Be(actualValue),
				_ => Assert.Fail("Should not be called"));
	}
	
	[Test]
	public void Option_None_To_Either()
	{
		Option<int> option = Of.None;
		var errorValue = "some error string";
		
		var either = option
			.ToEither(() => errorValue);

		either
			.Do(x => Assert.Fail("Should not be called"),
				s => s.Should().Be(errorValue));
	}

	[Test]
	public void Either_Result_To_OptionSome()
	{
		var someValue = _fixture.Create<int>();
		Either<int, string> either = someValue;
		either.AsOption()
			.Do(v => v.Should().Be(someValue), Assert.Fail);
	}
	
	[Test]
	public void Either_Error_To_OptionNone()
	{
		var errorValue = _fixture.Create<string>();
		Either<int, string> either = errorValue;
		either.AsOption()
			.Do(v => Assert.Fail(), Assert.Pass);
	}
}