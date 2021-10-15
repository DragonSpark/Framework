using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Model.Selection.Stores;

public class TableVariable<TIn, TOut> : IMutable<TOut?>
{
	readonly TIn                _key;
	readonly ITable<TIn, TOut?> _store;

	public TableVariable(TIn key, ITable<TIn, TOut?> store)
	{
		_key   = key;
		_store = store;
	}

	public TOut? Get() => _store.TryGet(_key, out var result) ? result : default;

	public void Execute(TOut? parameter)
	{
		_store.Assign(_key, parameter);
	}
}