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

	public static async Task  ComposeFunctionsWithEither(string inputFilePath)
	{
		Func<string, Task<string>> readFileFunc = path=> System.IO.File.ReadAllTextAsync(path);
		Func<string, Task<Either<string, Exception>>> tryReadFileFunc = filePath =>
			readFileFunc
				.Apply(filePath)
				.TryInvoke();
		
		Func<string, int> resolveWordCounts = input => input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

		var combinedCall = tryReadFileFunc.ComposeWith(resolveWordCounts);

		var result = await combinedCall(inputFilePath)
			.MapRightAsync(e => $"Error reading file: {e.Message}")
			.MatchAsync(wordCount => $"Contains {wordCount} word(s)",
				errorText => errorText);

	}
}