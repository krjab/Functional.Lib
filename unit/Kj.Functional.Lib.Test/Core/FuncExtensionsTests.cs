using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Kj.Functional.Lib.Core;
using NUnit.Framework;

namespace Kj.Functional.Lib.Test.Core;

[TestFixture]
public class FuncExtensionsTests
{
	private Fixture _fixture = null!;

	[SetUp]
	public void Setup()
	{
		_fixture = new Fixture();
	}

	[FsCheck.NUnit.Property()]
	public void Compose(string? input)
	{
		Func<string?, int> func1 = s => s?.Length ?? 0;
		Func<int, bool> func2 = i => i > 0;

		Func<string?, bool> composed = func1.ComposeWith(func2);

		func2(func1(input)).Should().Be(composed(input));
	}
	
	[TestCase("")]
	[TestCase("abc")]
	[TestCase(null)]
	public async Task ComposeAsync(string? input)
	{
		Func<string?, int> func1 = s => s?.Length ?? 0;
		Func<int, Task<int>> func2 = Task.FromResult;

		Func<string?, Task<int>> composed = func1.ComposeWith(func2);

		var res1 = await func2(func1(input));
		var resComposed = await composed(input);
		res1.Should().Be(resComposed);
	}
	
	[TestCase("")]
	[TestCase("abc")]
	[TestCase(null)]
	public async Task ComposeAsync2(string? input)
	{
		Func<string?, Task<int>> func1 = s =>
		{
			int lenVal = s?.Length ?? 0;
			return Task.FromResult(lenVal);
		};
		Func<int, Task<int>> func2 = Task.FromResult;

		Func<string?, Task<int>> composed = func1.ComposeWith(func2);

		var resFunc1 = await func1(input);
		var res2Calls = await func2(resFunc1);
		var resComposed = await composed(input);
		res2Calls.Should().Be(resComposed);
	}
	
	[TestCase("")]
	[TestCase("not-empty")]
	[TestCase(null)]
	public async Task ComposeAsync3(string? input)
	{
		Func<string?, Task<int>> func1 = s =>
		{
			int lenVal = s?.Length ?? 0;
			return Task.FromResult(lenVal);
		};
		Func<int, int> func2 = i=>i*2;

		var composed = func1.ComposeWith(func2);

		var resFunc1 = await func1(input);
		var res2Calls = func2(resFunc1);
		var resComposed = await composed(input);
		res2Calls.Should().Be(resComposed);
	}
	
	
	[FsCheck.NUnit.Property()]
	public void Compose_With_Option(string? input)
	{
		Func<string?, Option<int>> func1 = s => s!=null? s.Length:Of.None;
		Func<int, int> func2 = i => i * 2;

		Func<string?, Option<int>> composed = func1.ComposeWith(func2);

		const int invalidValue = -1;
		
		if (input != null)
		{
			var fromFunctions = func1(input)
				.Match(v => func2(v), () => invalidValue);
			var fromComposed = composed(input)
				.Match(v => v, () => invalidValue);

			fromFunctions.Should().Be(fromComposed);
		}
		else
		{
			composed(input)
				.Match(_ => _, () => invalidValue)
				.Should().Be(invalidValue);
		}
	}

	[Test]
	public void TryCall_Exceptionable([Values(true, false)] bool shouldThrow)
	{
		const string exceptionMessage = "Generated exception";

		Func<int> func = () =>
		{
			if (shouldThrow)
			{
				throw new Exception(exceptionMessage);
			}

			return _fixture.Create<int>();
		};

		var exceptionableResult = func.TryCall();
		if (shouldThrow)
		{
			exceptionableResult.Do(_ => Assert.Fail("Should have an exception"),
				_ => Assert.Pass());
		}
		else
		{
			exceptionableResult.Do(_ => Assert.Pass(),
				e => Assert.Fail(e.Message));
		}
		
	}
}