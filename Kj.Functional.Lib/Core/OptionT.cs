using System.Collections;

namespace Kj.Functional.Lib.Core
{
	public readonly struct Option<T> : IEnumerable<T>
	{
		private readonly bool _isSome;
		private readonly T[] _value;
		private Option(T value)
		{
			_isSome = true;
			this._value = new []{value};
		}

		public Option()
		{
			_isSome = false;
			_value = Array.Empty<T>();
		}

		public bool HasValue => _isSome;
		
		public static implicit operator Option<T>(Option.None _)
			=> new Option<T>();
		public static implicit operator Option<T>(Option.Some<T> some)
			=> new Option<T>(some.Value);
		public static implicit operator Option<T>(T value)
			=> value == null ? Of.None : Of.Some(value);
		
		// ReSharper disable once InconsistentNaming
		public R Match<R>(Func<T, R> Some,Func<R> None)
			=> _isSome ? Some(_value[0]) : None();

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return ((IEnumerable<T>)_value).GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return _value.GetEnumerator();
		}
	}
}


