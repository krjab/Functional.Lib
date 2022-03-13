using System;
using AutoFixture;
using FluentAssertions;
using Kj.Functional.Lib.Core;
using NUnit.Framework;
using Kj.Functional.Lib.Test.TestHelpers;

namespace Kj.Functional.Lib.Test.Core;

[TestFixture]
public class OptionApplyTests
{
	private Fixture _fixture = null!;
	
	[SetUp]
	public void Setup()
	{
		_fixture = new Fixture();
	}
	
	[FsCheck.NUnit.Property()]
	public void Apply_Another_Option(int a, int b)
	{
		Option<int> someA = a;
		Option<int> someB = b;
		
		Func<int, int> CreateMultiplyFunc(int x)
		{
			return y => x * b;
		}

		someA.Map(CreateMultiplyFunc)
			.Apply(someB)
			.Do(x => x.Should().Be(a*b), Assert.Fail);
	}

	[Test]
	public void MapToFunc_And_Apply_Another_Some()
	{
		Func<string, string> ConcatFunc(string s1) => s2 => $"{s1}{s2}";

		Option<string> optA = "a";
		Option<string> optB = "b";

		optA.Map(ConcatFunc)
			.Apply(optB)
			.Match(x => x, () => String.Empty)
			.Should().Be("ab");
	}
	
	[Test]
	public void Option_Applied_To_Option_Func()
	{
		Func<string, string, string> concatFunc = (s1, s2) => $"{s1}{s2}";
		Option<Func<string, string, string>> inputOptionFunc = concatFunc;

		string providedOptional1 = _fixture.Create<string>();
		string providedOptional2 = _fixture.Create<string>();
		Option<string> anotherOptional = providedOptional1;

		inputOptionFunc.Apply(anotherOptional)
			.Match(f => f(providedOptional2),
				() => "not expected")
			.Should().Be(concatFunc(providedOptional1, providedOptional2));
	}
	
	[FsCheck.NUnit.Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
	public void RightIdentityHolds(Option<object> m)
	{
		Func<object, Option<object>> bindFunc = i => Of.Some(i);
		Assert.AreEqual(m, m.Bind(bindFunc));
	}

}
