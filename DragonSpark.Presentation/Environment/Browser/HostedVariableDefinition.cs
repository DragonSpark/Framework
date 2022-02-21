using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Environment.Browser;

public class HostedVariableDefinition<T> : IHostedVariableDefinition<T>
{
	readonly string             _key;
	readonly IHostedVariable<T> _store;

	protected HostedVariableDefinition(Type key, ProtectedBrowserStorage storage)
		: this(key.AssemblyQualifiedName.Verify(), storage) {}

	protected HostedVariableDefinition(string key, ProtectedBrowserStorage storage)
		: this(key, new HostedVariable<T>(storage)) {}

	protected HostedVariableDefinition(string key, IHostedVariable<T> store)
		: this(key, store, new Operation(store.Remove.Then().Bind(key))) {}

	protected HostedVariableDefinition(string key, IHostedVariable<T> store, IOperation remove)
	{
		Remove = remove;
		_key   = key;
		_store = store;
	}

	public ValueTask<ProtectedBrowserStorageResult<T>> Get() => _store.Get(_key);

	public ValueTask Get(T parameter) => _store.Get((_key, parameter));

	public IOperation Remove { get; }
}