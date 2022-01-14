using Kj.Functional.Lib.Core;

namespace Kj.Functional.Lib.Extensions.Parse;

public static class ParseHelperExtensions
{
	public static Option<int> TryParseInt(this string input)
	{
		if (Int32.TryParse(input, out int res))
		{
			return res;
		}

		return Of.None;
	}
}