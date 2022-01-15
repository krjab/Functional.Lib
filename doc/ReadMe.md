### Welcome to the Functional.Lib wiki!

# Description
A simple library of structures and extension to enable writing c# code in a more functional style.
Avoid having to check for null values with usage of _Option_ and _Either_.

# Code examples

## How to use an _Option_:
`

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

`

## How to use an _Either_:


		string stringToParse = "123";

       // ParseString return Either<int, string> (depending if successful or not)
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


	