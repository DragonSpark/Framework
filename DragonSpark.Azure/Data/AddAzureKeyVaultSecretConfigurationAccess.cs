using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Azure.Data;

public sealed class AddAzureKeyVaultSecretConfigurationAccess : ICommand<HostingInput>
{
	public static AddAzureKeyVaultSecretConfigurationAccess Default { get; } = new();

	AddAzureKeyVaultSecretConfigurationAccess() : this(SecretClients.Default) {}

	readonly ISelect<HostingInput, SecretClient> _clients;

	public AddAzureKeyVaultSecretConfigurationAccess(ISelect<HostingInput, SecretClient> clients)
		=> _clients = clients;

	public void Execute(HostingInput parameter)
	{
		var (_, _, builder) = parameter;
		var client = _clients.Get(parameter);
		builder.AddAzureKeyVault(client, new KeyVaultSecretManager());
	}
}