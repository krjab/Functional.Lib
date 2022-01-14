using FluentAssertions;
using Kj.Functional.Lib.Extensions.Parse;
using NUnit.Framework;


namespace Kj.Functional.Lib.Extensions.Test;

[TestFixture]
public class ParseHelperTests
{
	[TestCase("1")]
	[TestCase("223342")]
	[TestCase("-1")]
	public void Parse_Int_Success(string input)
	{
		input.TryParseInt().Match(r => true, () => false).Should().Be(true);
	}
	
	[TestCase("")]
	[TestCase("  ")]
	[TestCase("some other string")]
	[TestCase("x1231")]
	public void Parse_Int_Fails(string input)
	{
		input.TryParseInt().Match(r => true, () => false).Should().Be(false);
	}
}