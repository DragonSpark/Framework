namespace DragonSpark.Model.Results;

public class Maybe<T> : IResult<T?>
{
	readonly IResult<T?> _first;
	readonly IResult<T?> _second;

	public Maybe(IResult<T?> first, IResult<T?> second)
	{
		_first  = first;
		_second = second;
	}

	public T? Get() => _first.Get() ?? _second.Get();
}