using Azure.Security.KeyVault.Secrets;
using DragonSpark.Model.Selection;

namespace DragonSpark.Azure.Data;

sealed class Accessors : ISelect<SecretClient, IKeyVaultVariableAccessor>
{
	public static Accessors Default { get; } = new();

	Accessors() {}

	public IKeyVaultVariableAccessor Get(SecretClient parameter) => new KeyVaultVariableAccessor(parameter);
}