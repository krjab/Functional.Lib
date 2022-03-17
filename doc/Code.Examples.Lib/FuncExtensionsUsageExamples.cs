using Kj.Functional.Lib.Core;

namespace Code.Examples.Lib;

public class FuncExtensionsUsageExamples
{
	public static void ComposeFunctions()
	{
		Func<string, string> readFileFunc = filePath => System.IO.File.ReadAllText(filePath);
		Func<string, int> resolveWordCounts = input => input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

		Func<string, int> combinedFuncReadAndResolveWordCount = readFileFunc.ComposeWith(resolveWordCounts);

		var lengthOfTextFile = combinedFuncReadAndResolveWordCount("input/file/path");
	}

	public static void ComposeFunctionsWithEither(string inputFilePath)
	{
		Func<string, string> readFileFunc = System.IO.File.ReadAllText;
		Func<string, Either<string, Exception>> tryReadFileFunc = filePath =>
			readFileFunc
				.Apply(filePath)
				.TryCall();
		
		Func<string, int> resolveWordCounts = input => input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

		var combinedCall = tryReadFileFunc.ComposeWith(resolveWordCounts);

		var result = combinedCall(inputFilePath)
			.MapError(e => $"Error reading file: {e.Message}")
			.Match(wordCount => $"Contains {wordCount} word(s)",
				errorText => errorText);

	}
}