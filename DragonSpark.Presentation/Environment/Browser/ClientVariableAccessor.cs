using DragonSpark.Model;
using DragonSpark.Presentation.Components.State;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public sealed class ClientVariableAccessor<T> : IClientVariableAccessor<T>
{
	readonly ProtectedBrowserStorage _store;

	public ClientVariableAccessor(ProtectedBrowserStorage store) : this(store, new Remove(store)) {}

	public ClientVariableAccessor(ProtectedBrowserStorage store, IRemove remove)
	{
		_store = store;
		Remove = remove;
	}

	public IRemove Remove { get; }

	public ValueTask Get(Pair<string, T> parameter) => _store.SetAsync(parameter.Key, parameter.Value!);

	public ValueTask<ProtectedBrowserStorageResult<T>> Get(string parameter) => _store.GetAsync<T>(parameter);
}