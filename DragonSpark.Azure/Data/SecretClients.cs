using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;
using System;

namespace DragonSpark.Azure.Data;

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