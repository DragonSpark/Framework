using Azure.Core;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Security.KeyVault.Secrets;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Configuration;
using System;

namespace DragonSpark.Azure.Data;

public sealed class AddAzureKeyVault : ICommand<HostingInput>
{
	public static AddAzureKeyVault Default { get; } = new();

	AddAzureKeyVault() : this(DefaultCredential.Default) {}

	readonly TokenCredential _credential;

	public AddAzureKeyVault(TokenCredential credential) => _credential = credential;

	public void Execute(HostingInput parameter)
	{
		var (_, context, builder) = parameter;
		var configuration = context.Configuration.Section<ProtectedConfiguration>() ??
		                    builder.Build().Section<ProtectedConfiguration>().Verify();
		var client = new SecretClient(new Uri(configuration.Location), _credential);
		builder.AddAzureKeyVault(client, new KeyVaultSecretManager());
	}
}