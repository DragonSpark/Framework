using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	public class SessionVariable<T> : ISessionVariable<T>
	{
		readonly ProtectedSessionStorage _store;

		public SessionVariable(ProtectedSessionStorage store) : this(store, new Remove(store)) {}

		public SessionVariable(ProtectedSessionStorage store, IRemove remove)
		{
			_store = store;
			Remove = remove;
		}

		public IRemove Remove { get; }

		public ValueTask Get((string Key, T Value) parameter) => _store.SetAsync(parameter.Key, parameter.Value!);

		public ValueTask<ProtectedBrowserStorageResult<T>> Get(string parameter) => _store.GetAsync<T>(parameter);
	}
}