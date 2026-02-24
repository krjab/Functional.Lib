using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Kj.Functional.Lib.Core
{
	[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types")]
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
		[SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix")]
        public R Match<R>(Func<T, R> some,Func<R> none)
			=> _isSome ? some(_value[0]) : none();

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


