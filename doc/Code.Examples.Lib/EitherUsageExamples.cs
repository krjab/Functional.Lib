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
			.DoWithResult(lv=>Console.WriteLine($"Correct value of {lv}"))
			.DoWithError(rv=>Console.WriteLine($"error: {rv}"))
			//
			.Match(
				i => $"Length of {i}",
				errorText => $"Parse error: {errorText}");
		
	}

	private record UserDto(string FirstName, string LastName);

	private record ErrorInfo(string ErrorText);
	
	public async Task UseChainedEitherMethods()
	{
		// An example Parse method (hardcoded logic)
		Task<Either<UserDto, ErrorInfo>> ParseAsync(string input)
		{
			Either<UserDto, ErrorInfo> result = new UserDto("first", "last");
			return Task.FromResult(result);
		}
		
		// An example Validate method (hardcoded logic)
		Task<Either<UserDto, ErrorInfo>> ValidateAsync(UserDto user)
		{
			Either<UserDto, ErrorInfo> result = new ErrorInfo("parse info");
			return Task.FromResult(result);
		}

		Task<Either<UserDto, ErrorInfo>> ModifyUserAsync(UserDto user)
		{
			Either<UserDto, ErrorInfo> result = user with{ FirstName = $"Modified {user.FirstName}"};
			return Task.FromResult(result);
		}
		
		string inputString = "abc";

		// Obtaining the result from the combined (chained) calls to consecutive methods.
		// Each method gets called only if the preceeding one succeeds.
		var combinedResult = await ParseAsync(inputString)
			.BindResultAsync(ValidateAsync)
			.BindResultAsync(ModifyUserAsync);
		
		// Now can use the result after the chained calls:
		var finalResult = combinedResult
			.Match(user => $"Processed user: {user.FirstName}",
				err => $"Error: {err.ErrorText}");

	}
}