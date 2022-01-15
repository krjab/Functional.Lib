using Kj.Functional.Lib.Core;

namespace Code.Examples.Lib;

public class EitherUsageExamples
{
	public static void UseAndConsumeEither()
	{
		Either<int, string> ParseString(string input)
		{
			if (input.Length > 0)
			{
				// return Either with a valid value (in this case: string's length).
				// (the conversion takes place implicitly)
				return input.Length;
			}

			// Returns Either with the 'right hand' value, in this case - error message
			// (the conversion takes place implicitly)
			return "String is empty";
		}

		string stringToParse = "123";

		var parseResult = ParseString(stringToParse);
		var finalResult = parseResult
			// we can (although we don't have to) utilize the value contained in Either for example to log
			.Do(
				lv=>Console.WriteLine($"Correct value of {lv}"),
				rv=>Console.WriteLine($"error: {rv}")
				)	
			//
			.Match(
				i => $"Length of {i}",
				errorText => $"Parse error: {errorText}");
		
	}
}