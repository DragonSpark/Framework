using Azure.Security.KeyVault.Secrets;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure.Data;

sealed class AddAzureKeyVaultSecret : ICommand<IServiceCollection>
{
	public static AddAzureKeyVaultSecret Default { get; } = new();

	AddAzureKeyVaultSecret() : this(SecretClients.Default) {}

	readonly ISelect<IConfiguration, SecretClient> _client;

	public AddAzureKeyVaultSecret(ISelect<IConfiguration, SecretClient> client) => _client = client;

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddSingleton(_client.Get(parameter.Configuration()));
	}
}