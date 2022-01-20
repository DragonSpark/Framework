using Azure.Core;
using DragonSpark.Application.Data;
using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;

namespace DragonSpark.Azure.Data;

public sealed class RegisterAzureKeyVaultEncryptedColumnKeyStore : RegisterEncryptedColumnKeyStore
{
	public static RegisterAzureKeyVaultEncryptedColumnKeyStore Default { get; } = new();

	RegisterAzureKeyVaultEncryptedColumnKeyStore() : this(DefaultCredential.Default) {}

	public RegisterAzureKeyVaultEncryptedColumnKeyStore(TokenCredential credential)
		: this(new SqlColumnEncryptionAzureKeyVaultProvider(credential)) {}

	public RegisterAzureKeyVaultEncryptedColumnKeyStore(SqlColumnEncryptionAzureKeyVaultProvider provider)
		: base(SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, provider) {}
}