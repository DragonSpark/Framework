using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Runtime
{
	sealed class ScopedVariableStore<T> : IMutable<T?>
	{
		readonly string       _key;
		readonly IScopedTable _store;

		public ScopedVariableStore(string key, IScopedTable store)
		{
			_key   = key;
			_store = store;
		}

		public T? Get() => _store.TryGet(_key, out var existing)
			                   ? existing is null ? default : existing.To<T>()
			                   : default;

		public void Execute(T? parameter)
		{
			_store.Assign(_key, parameter);
		}
	}
}