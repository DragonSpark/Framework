namespace DragonSpark.Model.Results;

public class CoalesceStructure<T> : IResult<T> where T : struct
{
	readonly IResult<T?> _first;
	readonly IResult<T>  _second;

	public CoalesceStructure(IResult<T?> first, IResult<T> second)
	{
		_first  = first;
		_second = second;
	}

	public T Get() => _first.Get() ?? _second.Get();
}