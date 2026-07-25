using DragonSpark.Model.Results;

namespace DragonSpark.Runtime.Execution;

public class Local<T> : IMutable<T?>
{
	readonly ThreadLocal<T?> _store;

	protected Local() : this(new()) {}

	protected Local(ThreadLocal<T?> store) => _store = store;

	public T? Get() => _store.Value;

	public void Execute(T? parameter)
	{
		_store.Value = parameter;
	}
}