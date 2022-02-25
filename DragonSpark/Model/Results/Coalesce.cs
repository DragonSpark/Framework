namespace DragonSpark.Model.Results;

public class Coalesce<T> : IResult<T>
{
	readonly IResult<T?> _first;
	readonly IResult<T>  _second;

	protected Coalesce(IResult<T?> first, IResult<T> second)
	{
		_first  = first;
		_second = second;
	}

	public T Get() => _first.Get() ?? _second.Get();
}