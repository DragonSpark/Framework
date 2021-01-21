using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	public class SessionVariableDefinition<T> : ISessionVariableDefinition<T>
	{
		readonly string              _key;
		readonly ISessionVariable<T> _store;

		public SessionVariableDefinition(string key, ProtectedSessionStorage store)
			: this(key, new SessionVariable<T>(store)) {}

		public SessionVariableDefinition(string key, ISessionVariable<T> store)
			: this(key, store, new Operation(store.Remove.Then().Bind(key))) {}

		public SessionVariableDefinition(string key, ISessionVariable<T> store, IOperation remove)
		{
			Remove = remove;
			_key   = key;
			_store = store;
		}

		public ValueTask<ProtectedBrowserStorageResult<T>> Get() => _store.Get(_key);

		public ValueTask Get(T parameter) => _store.Get((_key, parameter));

		public IOperation Remove { get; }
	}
}