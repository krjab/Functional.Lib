using System;
using AutoFixture;
using Kj.Functional.Lib.Core;
using NUnit.Framework;

namespace Kj.Functional.Lib.Test.Core;

[TestFixture]
public class CallsChainingTests
{
	private Fixture _fixture = null!;
	
	[SetUp]
	public void Setup()
	{
		_fixture = new Fixture();
	}
	
	[TestCase(100, 200, "err")]
	[TestCase(100, "err", 123)]
	[TestCase("err", 333, 123)]
	[TestCase("err1", "err2", "err3")]
	public void Error_Result_Pulled(object result1, object result2, object result3)
	{
		var func1 = () => CreateEither(result1);
		Func<int, Either<int, string>> func2 = _ => CreateEither(result2);
		Func<int, Either<int, string>> func3 = _ => CreateEither(result3);
		
		var finalResult = func1()
			.BindResult(func2)
			.BindResult(func3);

		finalResult.Do(_ => Assert.Fail(),
			_ => Assert.Pass());
	}
	
	[TestCase(100, 200, "err", ExpectedResult = "err")]
	[TestCase(100, 200, 333, ExpectedResult = 333)]
	[TestCase(100, "err", 333, ExpectedResult = "err")]
	[TestCase("err", 22, 33, ExpectedResult = "err")]
	public object Result_Pulled(object result1, object result2, object result3)
	{
		var func1 = () => CreateEither(result1);
		Func<int, Either<int, string>> func2 = _ => CreateEither(result2);
		Func<int, Either<int, string>> func3 = _ => CreateEither(result3);
		
		var finalResult = func1()
			.BindResult(func2)
			.BindResult(func3);

		return finalResult.Match(i => (object)i, s => s);
	}

	private static Either<int, string> CreateEither(object input)
	{
		if (input is int v)
		{
			return Either<int, string>.Left(v);
		}

		return Either<int, string>.Right(input.ToString()!);
	}
}