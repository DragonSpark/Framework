using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public class ClientVariable<T> : IClientVariable<T>
{
	readonly string                     _key;
	readonly IClientVariableAccessor<T> _store;

	public ClientVariable(string key, ProtectedBrowserStorage storage)
		: this(key, new ConnectionAwareClientVariableAccessor<T>(new ClientVariableAccessor<T>(storage))) {}

	protected ClientVariable(Type key, ProtectedBrowserStorage storage)
		: this(key.AssemblyQualifiedName.Verify(), storage) {}

	protected ClientVariable(string key, IClientVariableAccessor<T> store)
		: this(key, store, new Operation(store.Remove.Then().Bind(key))) {}

	protected ClientVariable(string key, IClientVariableAccessor<T> store, IOperation remove)
	{
		Remove = remove;
		_key   = key;
		_store = store;
	}

	public ValueTask<ProtectedBrowserStorageResult<T>> Get() => _store.Get(_key);

	public ValueTask Get(T parameter) => _store.Get(Pairs.Create(_key, parameter));

	public IOperation Remove { get; }
}