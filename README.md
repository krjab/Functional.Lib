### Welcome to the Functional.Lib wiki!

# Description
A simple library of structures and extension to enable writing c# code in a more functional style.
Avoid having to check for null values with usage of _Option_ and _Either_.

# Most important structures
Two most important structures to use are
_Option_ and _Either_.

_Option_ represents something that may (but does not have to) exist.
An _Option_ can be one of two: _Some_ or _None_. Examples would be:
 - parsing result (valid parsed value or nothing)
 - search result (valid record found or nothing)

_Either_ represents one of two possible result, 'left' or 'right'.
'Left' can mostly be used to represent a valid result, the 'right' to 
represent an error that occured.


# Code examples

## How to use an _Option_:
```

     string inputString = "123";
    
    // parsedOptional contains either a value (123) or nothing, depending on the result of TryParse
    // in this case it's valid value
    var parsedOptional = inputString.TryParseInt();
    
    // if parsedOptional contains a value AND if this value satisfies the condition (>100)
    // then filtered becomes an optional with this value. Otherwise it gets to None.
    var filtered = parsedOptional.Filter(i => i > 100);

    // we can use do to utilize to value without a result, i.e. for example to log.
    filtered
    	.Do(v => Console.WriteLine("Valid value"),
    		() => Console.WriteLine("Value missing"));

    // at the end ot the processing, let's map the optional value into a something final
    var finalValue = filtered
    	.Match(v => $"correct value of {v}",
    		() => "Incorrect value");

```

## How to use an _Either_:

```
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
```

## Example of chained calls using _Either_:

Function calls returning Either can be chained, making the code
more readable and logically structured.
Important: as long the the methods that are being called consecutively stick to the same signature
they can be chained as in this example.

```
    
    string inputString = "ABC";
    // Obtaining the result from the combined (chained) calls to consecutive methods.
    // Each method gets called only if the preceeding one succeeds.
    var combinedResult = await ParseAsync(inputString)
    	.BindResultAsync(ValidateAsync)
    	.BindResultAsync(ModifyUserAsync);
    	
    // Now can use the result after the chained calls:
    var finalResult = combinedResult
    	.Match(user => $"Processed user: {user.FirstName}",
        	err => $"Error: {err.ErrorText}");

```

Any next function is called only if the previous one returns
the Either with the _'left hand'_ part (meaning valid result). Otherwise
the further methods do not get called and processing stops
with the _'right hand'_ value (meaning error result).

# Further development
I'll be happy to hear about suggestions or ideas regarding further features.


