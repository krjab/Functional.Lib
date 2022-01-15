using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Kj.Functional.Lib.Core;
using Kj.Functional.Lib.Test.TestHelpers.Fixture;
using NUnit.Framework;

namespace Kj.Functional.Lib.Test.Core;

[TestFixture]
public class EitherExtensionsTests
{
	private Fixture _fixture = null!;
	
	[SetUp]
	public void Setup()
	{
		_fixture = new Fixture();
	}

	[Test]
	public void MapLeft()
	{
		int val = _fixture.Create<int>();
		int valMapped = _fixture.Create<int>();
		Either<int, string> either = val;

		either.MapLeft(i=>valMapped).Match(i => i,
			_ =>
			{
				Assert.Fail();
				return -1;
			}).Should().Be(valMapped);
	}
	
	[Test]
	public void MapRight()
	{
		int val = _fixture.Create<int>();
		int valMapped = _fixture.Create<int>();
		
		Either<string, int> either = val;

		either.MapRight(i=>valMapped).Match(_ =>
				{
					Assert.Fail();
					return -1;
				},
				i => i)
			.Should().Be(valMapped);
	}
	
	
	[Test]
	public async Task MapLeftAsync()
	{
		int val = _fixture.Create<int>();
		int valMapped = _fixture.Create<int>();
		Either<int, string> either = val;

		Func<int, Task<int>> mapFunc = _ => Task.FromResult(valMapped);

		var mapRes = await either.MapLeftAsync(mapFunc);
			
			mapRes
			.Match(i => i,
			_ =>
			{
				Assert.Fail();
				return -1;
			}).Should().Be(valMapped);
	}

	[Test]
	public async Task MapTaskLeftAsync()
	{
		int val = _fixture.Create<int>();
		int valMapped = _fixture.Create<int>();
		Either<int, string> either = val;
		var eitherTask = Task.FromResult(either);

		Func<int, Task<int>> mapFunc = _ => Task.FromResult(valMapped);

		var mapRes = await eitherTask.MapLeftAsync(mapFunc)
			.MatchAsync(i => i,
				_ =>
				{
					Assert.Fail();
					return -1;
				});
			mapRes.Should().Be(valMapped);
	}
	
	
	[Test]
	public void BindLeft()
	{
		int val = _fixture.Create<int>();
		Either<int, string> boundVal = val;

		Func<decimal, Either<int, string>> bindFunc = _ => boundVal;
		
		Either<decimal, string> initial = 123M;
		initial.BindLeft(s=>bindFunc(s))
			.Match(i => i, _ =>
			{
				Assert.Fail();
				return -1;
			}).Should().Be(val);
	}
	
	[Test]
	public async Task BindLeftAsync()
	{
		int val = _fixture.Create<int>();
		Either<int, string> boundVal = val;

		Func<decimal, Task<Either<int, string>>> bindFunc = _ => Task.FromResult(boundVal);
		
		Either<decimal, string> initial = 123M;
		var bound = await initial.BindLeftAsync(s=>bindFunc(s));
			bound
			.Match(i => i, _ =>
			{
				Assert.Fail();
				return -1;
			}).Should().Be(val);
	}

	[Test]
	public async Task BindLeftAsync_WithTask()
	{
		int val = _fixture.CreateInt(100, 200);
		Either<int, string> boundVal = -12334;
		Either<int, string> boundVal2 = val;

		Func<decimal, Task<Either<int, string>>> bindFunc = _ => Task.FromResult(boundVal);
		Func<decimal, Task<Either<int, string>>> bindFunc2 = _ => Task.FromResult(boundVal2);
		
		Task<Either<decimal, string>> initialTask = Task.FromResult((Either<decimal,string>) 123M);

		var bound = await initialTask
			.BindLeftAsync(s => bindFunc(s))
			.BindLeftAsync(x => bindFunc2(x));
		
		bound
			.Match(i => i, _ =>
			{
				Assert.Fail();
				return -1;
			}).Should().Be(val);
	}
	
	[Test]
	public void Do_With_LeftResult()
	{
		int someValue = _fixture.Create<int>();
		Either<int, string> either = someValue;

		either
			.Do(i => i.Should().Be(someValue),
				_ => Assert.Fail());
	}
	
	[Test]
	public void Do_With_RightResult()
	{
		string someValue = _fixture.Create<string>();
		Either<int, string> either = someValue;

		either
			.Do(i => Assert.Fail(),
				s => s.Should().Be(someValue));
	}
}