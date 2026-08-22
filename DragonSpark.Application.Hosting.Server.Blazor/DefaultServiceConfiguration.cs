using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Blazor;

sealed class DefaultServiceConfiguration : ICommand<IServiceCollection>
{
	public static DefaultServiceConfiguration Default { get; } = new();

	DefaultServiceConfiguration() : this(32) {}

	readonly Action<CircuitOptions> _options;
	readonly byte                   _receive;

	public DefaultServiceConfiguration(byte receive) : this(_ => {}, receive) {}

	public DefaultServiceConfiguration(Action<CircuitOptions> options, byte receive)
	{
		_options = options;
		_receive = receive;
	}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddRazorComponents()
		         .AddInteractiveServerComponents(_options)
		         .AddHubOptions(x => x.MaximumReceiveMessageSize = _receive * 1024);
	}
}