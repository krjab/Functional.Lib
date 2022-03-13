using System;
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
}