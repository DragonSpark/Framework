using Azure.Security.KeyVault.Secrets;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Data;

public sealed class Remove : IRemove
{
	readonly SecretClient _store;

	public Remove(SecretClient store) => _store = store;

	public async ValueTask Get(string parameter)
		=> await _store.StartDeleteSecretAsync(parameter).ConfigureAwait(false);
}