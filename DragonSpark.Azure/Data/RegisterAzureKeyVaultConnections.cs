using Azure.Core;
using DragonSpark.Model.Commands;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;
using System;
using System.Collections.Generic;

namespace DragonSpark.Azure.Data;

sealed class RegisterAzureKeyVaultConnections : ICommand<TokenCredential>
{
	public static RegisterAzureKeyVaultConnections Default { get; } = new();

	RegisterAzureKeyVaultConnections() {}

	public void Execute(TokenCredential parameter)
	{
		var provider = new SqlColumnEncryptionAzureKeyVaultProvider(parameter);
		var providers = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>(1, StringComparer.OrdinalIgnoreCase)
		{
			{ SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, provider }
		};

		SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
	}
}