using System;

namespace DragonSpark.Model.Results;

public class Deferred<T> : IResult<T>
{
	public static implicit operator T(Deferred<T> source) => source.Get();

	readonly Lazy<T> _source;

	public Deferred(Func<T> source) : this(new Lazy<T>(source)) {}

	public Deferred(Lazy<T> source) => _source = source;

	public T Get() => _source.Value;
}