using DragonSpark.Model;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Runtime
{
	sealed class RemoveScopedVariable : ICommand
	{
		readonly string       _key;
		readonly IScopedTable _store;

		public RemoveScopedVariable(string key, IScopedTable store)
		{
			_key   = key;
			_store = store;
		}

		public void Execute(None parameter)
		{
			_store.Remove(_key);
		}
	}
}