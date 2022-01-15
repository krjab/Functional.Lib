using Option;

namespace Kj.Functional.Lib.Core;

public readonly struct Either<TL, TR>
{
	private readonly Option<TL> _leftVal;
	private readonly Option<TR> _rightVal;

	private Either(Option<TL> leftVal, Option<TR> rightVal)
	{
		AssetLeftAndRight(leftVal, rightVal);
		_leftVal = leftVal;
		_rightVal = rightVal;
	}

	private static void AssetLeftAndRight(Option<TL> leftVal, Option<TR> rightVal)
	{
		if (!leftVal.HasValue && !rightVal.HasValue)
		{
			throw new ArgumentException("Both values are none");
		}

		if (leftVal.HasValue && rightVal.HasValue)
		{
			throw new ArgumentException("Both values are some");
		}
	}

	public static Either<TL, TR> Left(TL leftVal)
	{
		return new Either<TL, TR>(leftVal, None.Default);
	} 
	
	public static Either<TL, TR> Right(TR rightVal)
	{
		return new Either<TL, TR>( None.Default, rightVal);
	}

	/// <summary>
	/// Maps the left or right side value to another target type.
	/// </summary>
	/// <param name="left">Left side value mapping function</param>
	/// <param name="right">Right side value mapping function</param>
	/// <typeparam name="T">Underlying type</typeparam>
	/// <returns>Instance of T</returns>
	/// <exception cref="ArgumentException">Throws an exception if the object has not been initialised correctly.</exception>
	public T Match<T>(Func<TL, T> left, Func<TR, T> right)
	{
		if (_leftVal.HasValue)
		{
			return _leftVal.Match(left, () => throw new ArgumentException("invalid left"));
		}
		
		return _rightVal.Match(right, () => throw new ArgumentException("invalid right"));
	}
	
	public static implicit operator Either<TL,TR>(TL left)
		=> Left(left);
	
	public static implicit operator Either<TL,TR>(TR right)
		=> Right(right);
}