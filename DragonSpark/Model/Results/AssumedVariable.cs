using DragonSpark.Compose;
using JetBrains.Annotations;

namespace DragonSpark.Model.Results;

[UsedImplicitly]
public class AssumedVariable<T> : IMutable<T?>
{
	readonly IResult<IMutable<T?>> _source;

	protected AssumedVariable(IResult<IMutable<T?>> source) => _source = source;

	public T? Get() => _source.Instance();

	public void Execute(T? parameter)
	{
		_source.Get().Execute(parameter);
	}
}