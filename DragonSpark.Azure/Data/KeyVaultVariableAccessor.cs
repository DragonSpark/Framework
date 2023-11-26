using Azure.Security.KeyVault.Secrets;
using DragonSpark.Model;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Data;

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