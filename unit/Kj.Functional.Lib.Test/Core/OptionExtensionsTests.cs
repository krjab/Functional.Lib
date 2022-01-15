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
public class OptionExtensionsTests
{
	private Fixture _fixture = null!;
	
	[SetUp]
	public void Setup()
	{
		_fixture = new Fixture();
	}

	[Test]
	public void Do_Some()
	{
		int testedVal = _fixture.CreateInt(0, 100);
		Option<int> option = testedVal;

		option.Do(r => r.Should().Be(testedVal), Assert.Fail);
	}
	
	[Test]
	public void Do_None()
	{
		Option<int> option = Of.None;

		option.Do(r => Assert.Fail(), Assert.Pass);
	}
	
	[Test]
	public void Map_Some()
	{
		int testedVal = _fixture.CreateInt(0, 100);
		Option<int> option = testedVal;
		string testedMapped = _fixture.Create<string>();

		var mapped = option.Map(x => testedMapped);
		mapped.Match(x => x == testedMapped, () => false).Should().BeTrue();
	}
	
	[Test]
	public void Map_None()
	{
		Option<int> option = Of.None;
		string testedMapped = _fixture.Create<string>();

		var mapped = option.Map(x => testedMapped);
		mapped.Match(x => x == testedMapped, () => false).Should().BeFalse();
	}

	[Test]
	public void Bind_Some()
	{
		Option<int> optionInt = _fixture.Create<int>();
		string testedVal = _fixture.Create<string>();
		
		Option<string> someStr = testedVal;
		var bound = optionInt.Bind(x => someStr);
		bound.Do(x => x.Should().Be(testedVal), Assert.Fail);
	}
	
	[Test]
	public async Task Bind_SomeAsync()
	{
		Option<int> optionInt = _fixture.Create<int>();
		string testedVal = _fixture.Create<string>();
		
		Option<string> someStr = testedVal;
		Func<int, Task<Option<string>>> bindFunc = i => Task.FromResult(someStr);
		
		var bound = await optionInt.BindAsync(x => bindFunc(x));
		bound.Do(x => x.Should().Be(testedVal), Assert.Fail);
	}
	
	[Test]
	public void Bind_None()
	{
		Option<int> optionInt = Of.None;
		string testedVal = _fixture.Create<string>();
		
		Option<string> someStr = testedVal;
		var bound = optionInt.Bind(x => someStr);
		bound.Do(x => Assert.Fail(), Assert.Pass);
	}

	[Test]
	public void Bind_From_List()
	{
		Option<int> op1 = _fixture.Create<int>();
		Option<int> op2 = _fixture.Create<int>();
		Option<int> none = Of.None;
		var array = new[] { op1, op2, none };
		var flattened = array.Bind().ToArray();
		flattened.Length.Should().Be(2);
	}

	[Test]
	public void Bind_To_Either()
	{
		Either<int, string> BindFunc(int input)
		{
			return input * 2;
		}

		int someVal = _fixture.Create<int>();
		Option<int> some = someVal;

		some
			.Bind(i=>BindFunc(i),()=>"err")
			.Do(vl => vl.Should().Be(someVal * 2),
				vr => Assert.Fail());
	}

	[Test]
	public void Where()
	{
		const int testedVal = 100;
		Option<int> op1 = testedVal;

		op1.Filter(x => x == testedVal).Match(x => true, () => false).Should().BeTrue();
		op1.Filter(x => x < testedVal).Match(x => true, () => false).Should().BeFalse();
	}
}