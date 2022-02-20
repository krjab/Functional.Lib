using System.Globalization;
using Kj.Functional.Lib.Core;

namespace Code.Examples.Lib;

internal static class Helpers
{
	public static Option<int> TryParseInt(this string input)
	{
		if (int.TryParse(input, out int res))
		{
			return res;
		}

		return Of.None;
	}
	
}