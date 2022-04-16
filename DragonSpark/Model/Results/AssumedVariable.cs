namespace DragonSpark.Model.Results;

public class AssumedVariable<T> : IMutable<T?>
{
	readonly IResult<IMutable<T?>> _source;

	protected AssumedVariable(IResult<IMutable<T?>> source) => _source = source;

	public T? Get() => _source.Get().Get();

	public void Execute(T? parameter)
	{
		_source.Get().Execute(parameter);
	}
}