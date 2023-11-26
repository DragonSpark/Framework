using Azure.Security.KeyVault.Secrets;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Data;

public class KeyVaultVariable : IKeyVaultVariable
{
	readonly string                    _key;
	readonly IKeyVaultVariableAccessor _store;

	public KeyVaultVariable(string key, SecretClient storage) : this(key, Accessors.Default.Get(storage)) {}

	protected KeyVaultVariable(Type key, SecretClient storage) : this(key.AssemblyQualifiedName.Verify(), storage) {}

	protected KeyVaultVariable(string key, IKeyVaultVariableAccessor store)
		: this(key, store, store.Remove.Then().Bind(key).Out()) {}

	protected KeyVaultVariable(string key, IKeyVaultVariableAccessor store, IOperation remove)
	{
		Remove = remove;
		_key   = key;
		_store = store;
	}

	public async ValueTask<string?> Get()
	{
		var store = await _store.Get(_key);
		return store?.Value;
	}

	public ValueTask Get(string parameter) => _store.Get((_key, parameter));

	public IOperation Remove { get; }
}