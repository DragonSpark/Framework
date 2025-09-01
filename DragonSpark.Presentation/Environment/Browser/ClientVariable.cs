using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public class ClientVariable<T> : IClientVariable<T>
{
	readonly string                     _key;
	readonly IClientVariableAccessor<T> _store;

	protected ClientVariable(Type key, ProtectedBrowserStorage storage) : this(key.FullName.Verify(), storage) {}

	public ClientVariable(string key, ProtectedBrowserStorage storage)
		: this(key, ClientAccessors<T>.Default.Get(storage)) {}

	protected ClientVariable(string key, IClientVariableAccessor<T> store)
		: this(key, store, store.Remove.Then().Bind(key).Out()) {}

	protected ClientVariable(string key, IClientVariableAccessor<T> store, IOperation remove)
	{
		Remove = remove;
		_key   = key;
		_store = store;
	}

	public ValueTask<ProtectedBrowserStorageResult<T>> Get() => _store.Get(_key);

	public ValueTask Get(T parameter) => _store.Get((_key, parameter));

	public IOperation Remove { get; }
}