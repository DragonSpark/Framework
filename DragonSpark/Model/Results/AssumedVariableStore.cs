namespace DragonSpark.Model.Results;

public class AssumedVariableStore<T> : IMutable<T?>
{
	readonly IMutable<IMutable<T?>?> _variable;

	protected AssumedVariableStore(IMutable<IMutable<T?>?> variable) => _variable = variable;

	public void Execute(T? parameter)
	{
		_variable.Get()?.Execute(parameter);
	}

	public T? Get()
	{
		var current = _variable.Get();
		var result  = current is not null ? current.Get() : default;
		return result;
	}
}