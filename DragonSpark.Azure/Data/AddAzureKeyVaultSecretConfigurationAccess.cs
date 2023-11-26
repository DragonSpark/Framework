using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

// TODO

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

sealed class SecretClients : ISelect<Uri, SecretClient>,
                             ISelect<IConfiguration, SecretClient>,
                             ISelect<HostingInput, SecretClient>
{
	public static SecretClients Default { get; } = new();

	SecretClients() : this(DefaultCredential.Default, new()
	{
		Retry =
		{
			Delay      = TimeSpan.FromSeconds(2),
			MaxDelay   = TimeSpan.FromSeconds(16),
			MaxRetries = 5,
			Mode       = RetryMode.Exponential
		}
	}) {}

	readonly TokenCredential     _credential;
	readonly SecretClientOptions _options;

	public SecretClients(TokenCredential credential, SecretClientOptions options)
	{
		_credential = credential;
		_options    = options;
	}

	public SecretClient Get(Uri parameter) => new(parameter, _credential, _options);

	public SecretClient Get(ProtectedConfiguration parameter) => Get(new Uri(parameter.Location));

	public SecretClient Get(IConfiguration parameter) => Get(parameter.Section<ProtectedConfiguration>().Verify());

	public SecretClient Get(HostingInput parameter)
	{
		var (_, context, builder) = parameter;
		var configuration = context.Configuration.Section<ProtectedConfiguration>()
		                    ??
		                    builder.Build().Section<ProtectedConfiguration>().Verify();
		return Get(configuration);
	}
}


