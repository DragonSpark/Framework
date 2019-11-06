using System;

namespace DragonSpark.Model.Results
{
	public class Assume<T> : IResult<T>
	{
		readonly Func<Func<T>> _result;

		public Assume(Func<Func<T>> result) => _result = result;

		public T Get() => _result()();

		public static implicit operator T(Assume<T> result) => result.Get();
	}
}