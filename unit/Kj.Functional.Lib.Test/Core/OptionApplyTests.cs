using System;
using AutoFixture;
using FluentAssertions;
using FsCheck;
using Kj.Functional.Lib.Core;
using NUnit.Framework;
using FsCheck.NUnit;
using Option;

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
	
	[FsCheck.NUnit.Property(Arbitrary = new[] { typeof(ArbitraryOption) })]
	public void RightIdentityHolds(Option<object> m)
	{
		Func<object, Option<object>> bindFunc = i => Of.Some(i);
		Assert.AreEqual(m, m.Bind(bindFunc));
	}

}
