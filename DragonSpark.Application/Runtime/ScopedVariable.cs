using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Runtime
{
	public class ScopedVariable<T> : IScopedVariable<T?>
	{
		readonly string       _key;
		readonly IScopedTable _store;

		public ScopedVariable(string key, IScopedTable store)
			: this(key, store, store.Condition.Then().Bind(key).Out()) {}

		public ScopedVariable(string key, IScopedTable store, ICondition<None> condition)
		{
			_key      = key;
			_store    = store;
			Condition = condition;
		}

		public T? Get() => _store.TryGet(_key, out var existing)
			                   ? existing is null ? default : existing.To<T>()
			                   : default;

		public void Execute(T? parameter)
		{
			_store.Assign(_key, parameter);
		}

		public void Execute(None parameter)
		{
			_store.Remove(_key);
		}

		public ICondition<None> Condition { get; }
	}
}