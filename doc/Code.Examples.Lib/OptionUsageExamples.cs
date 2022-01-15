using Kj.Functional.Lib.Core;
using Kj.Functional.Lib.Extensions.Parse;

namespace Code.Examples.Lib;

public class OptionUsageExamples
{
	public static void UseAndConsumeOption()
	{
		// List of strings to parse
		string[] toParse = new[]
		{
			"1",
			"2",
			"not-valid",
			"4",
			"abc"
		};
		
		IEnumerable<int> parsed = toParse
			// Try to convert string to int => as Option<int>
			.Select(s => s.TryParseInt())
			// Bind converts Options containing a value to the actual value
			.Bind();
		
		// The result object contains valid values.
	}

	public static void ConsumeOptionExample2()
	{
		string inputString = "123";
		
		// parsedOptional contains either a value (123) or nothing, depending on the result of TryParse
		// in this case it's valid value
		var parsedOptional = inputString.TryParseInt();
		
		// if parsedOptional contains a value AND if this value satisfies the condition (>100)
		// then filtered becomes an optional with this value. Otherwise it gets to None.
		var filtered = parsedOptional.Where(i => i > 100);

		// we can use do to utilize to value without a result, i.e. for example to log.
		filtered
			.Do(v => Console.WriteLine("Valid value"),
				() => Console.WriteLine("Value missing"));

		// at the end ot the processing, let's map the optional value into a something final
		var finalValue = filtered
			.Match(v => $"correct value of {v}",
				() => "Incorrect value");
		
	}
}