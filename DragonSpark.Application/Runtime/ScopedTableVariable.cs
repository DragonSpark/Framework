using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Runtime
{
	public class ScopedTableVariable<T> : IMutable<T?> where T : class
	{
		readonly IMutable<object?> _store;

		public ScopedTableVariable(IScopedTable table) : this(A.Type<T>().AssemblyQualifiedName.Verify(), table) {}

		public ScopedTableVariable(string key, IScopedTable table)
			: this(new TableVariable<string, object?>(key, table)) {}

		public ScopedTableVariable(IMutable<object?> store) => _store = store;

		public T? Get() => _store.Get()?.To<T>();

		public void Execute(T? parameter)
		{
			_store.Execute(parameter);
		}
	}
}