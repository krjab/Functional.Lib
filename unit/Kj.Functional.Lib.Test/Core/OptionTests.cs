using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Kj.Functional.Lib.Core;
using Kj.Functional.Lib.Test.TestHelpers.Fixture;
using NUnit.Framework;

namespace Kj.Functional.Lib.Test.Core;

[TestFixture]
public class OptionTests
{
	private Fixture _fixture = null!;
	
	[SetUp]
	public void Setup()
	{
		_fixture = new Fixture();
	}

	[Test]
	public void Option_With_Value()
	{
		int initialVal = _fixture.CreateInt(10, 100);
		Option<int> op = initialVal;

		op
			.Match(v => v,() => -1 ).Should().Be(initialVal);
	}

	[Test]
	public void Option_WithOut_Value()
	{
		Option<int> op = Of.None;
		op.Match(v => 10,() => -1).Should().Be(-1);
	}

	[Test]
	public void Option_With_Nullable_Value()
	{
		Option<int?> op = Of.Some<int?>((int?)null);
		op.Match(v => true, () => false).Should().BeTrue();
	}
	
	[Test]
	public void Option_With_NotNullable_Value()
	{
		Option<string> op = null!;
		op.Match(v => true, () => false).Should().BeFalse();
	}

	[Test]
	public void Option_Can_Be_Enumerated()
	{
		Option<int> op = _fixture.Create<int>();
		op.ToArray().Should().NotBeEmpty();
	}
	
	[Test]
	public void ForEach_With_Value()
	{
		int initialVal = _fixture.CreateInt(10, 100);
		Option<int> op = initialVal;

		var mutable = -1;
		op.ForEach(x=>mutable=x);
		mutable.Should().Be(initialVal);
	}
	
	[Test]
	public async Task ForEachAsync_With_Value()
	{
		int initialVal = _fixture.CreateInt(10, 100);
		Option<int> op = initialVal;
		

		var mutable = -1;
		Func<int, Task> asyncAction = i =>
		{
			mutable = i;
			return Task.CompletedTask;
		};
			
		await op.ForEachAsync(x=>asyncAction(x));
		mutable.Should().Be(initialVal);
	}
	
	[Test]
	public void ForEach_With_None()
	{
		int initialVal = _fixture.CreateInt(10, 100);
		Option<int> op = Of.None;

		var mutable = -1;
		op.ForEach(x=>mutable=x);
		mutable.Should().Be(-1);
	}
}