using System;
using AutoFixture;
using FluentAssertions;
using Kj.Functional.Lib.Core;
using NUnit.Framework;

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
	
	
	[Test]
	public void Apply_Another_Option()
	{
		Option<int> some3 = 3;
		Option<int> some10 = 10;
		
		Func<int, int> CreateMultiplyFunc(int a)
		{
			return b => a * b;
		}

		some3.Map(CreateMultiplyFunc)
			.Apply(some10)
			.Do(x => x.Should().Be(30), Assert.Fail);

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

}