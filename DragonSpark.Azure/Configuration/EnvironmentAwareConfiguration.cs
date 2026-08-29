using DragonSpark.Application.Configuration;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure.Configuration;

sealed class EnvironmentAwareConfiguration : ICommand<IServiceCollection>
{
	public static EnvironmentAwareConfiguration Default { get; } = new();

	EnvironmentAwareConfiguration() : this(AzureTenant.Default, AzureClient.Default, AzureSecret.Default) {}

	readonly IAssign _tenant, _client, _secret;

	public EnvironmentAwareConfiguration(IAssign tenant, IAssign client, IAssign secret)
	{
		_tenant = tenant;
		_client = client;
		_secret = secret;
	}

	public void Execute(IServiceCollection parameter)
	{
		var configuration = parameter.Configuration();
		_tenant.Execute(configuration);
		_client.Execute(configuration);
		_secret.Execute(configuration);
	}
}