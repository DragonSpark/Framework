using Azure.Security.KeyVault.Secrets;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Data;

public interface IKeyVaultVariableAccessor : ISelecting<string, KeyVaultSecret?>, IOperation<Pair<string, string>>
{
	IRemove Remove { get; }
}

sealed class Accessors : ISelect<SecretClient, IKeyVaultVariableAccessor>
{
	public static Accessors Default { get; } = new();

	Accessors() {}

	public IKeyVaultVariableAccessor Get(SecretClient parameter) => new KeyVaultVariableAccessor(parameter);
}

public sealed class KeyVaultVariableAccessor : IKeyVaultVariableAccessor
{
	readonly SecretClient _store;

	public KeyVaultVariableAccessor(SecretClient store) : this(store, new Remove(store)) {}

	public KeyVaultVariableAccessor(SecretClient store, IRemove remove)
	{
		_store = store;
		Remove = remove;
	}

	public IRemove Remove { get; }

	public async ValueTask Get(Pair<string, string> parameter)
	{
		await _store.SetSecretAsync(parameter.Key, parameter.Value).ConfigureAwait(false);
	}

	public async ValueTask<KeyVaultSecret?> Get(string parameter)
	{
		var result = await _store.GetSecretAsync(parameter).ConfigureAwait(false);
		return result;
	}
}

public interface IRemove : IOperation<string>;

public sealed class Remove : IRemove
{
	readonly SecretClient _store;

	public Remove(SecretClient store) => _store = store;

	public async ValueTask Get(string parameter)
		=> await _store.StartDeleteSecretAsync(parameter).ConfigureAwait(false);
}

public interface IKeyVaultVariable : IResulting<string?>, IOperation<string>
{
	IOperation Remove { get; }
}

public class KeyVaultVariable : IKeyVaultVariable
{
	readonly string                     _key;
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