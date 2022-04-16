namespace DragonSpark.Model.Results;

public class DeferredVariable<T> : IMutable<T?>
{
	readonly IResult<IMutable<T?>> _source;

	protected DeferredVariable(IResult<IMutable<T?>> source) => _source = source;

	public T? Get() => _source.Get().Get();

	public void Execute(T? parameter)
	{
		_source.Get().Execute(parameter);
	}
}