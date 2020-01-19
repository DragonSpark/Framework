using System;

namespace DragonSpark.Model.Results
{
	public class DeferredSingleton<T> : IResult<T>
	{
		public static implicit operator T(DeferredSingleton<T> source) => source.Get();

		readonly Lazy<T> _source;

		public DeferredSingleton(Func<T> source) : this(new Lazy<T>(source)) {}

		public DeferredSingleton(Lazy<T> source) => _source = source;

		public T Get() => _source.Value;
	}
}