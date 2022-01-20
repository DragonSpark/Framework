using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

public sealed class ConfigureHostBuilderFromEnvironment : ICommand<IHostBuilder>
{
	public static ConfigureHostBuilderFromEnvironment Default { get; } = new();

	ConfigureHostBuilderFromEnvironment() : this(ConfigureHostBuilderFromEnvironmentCommand.Default) {}

	readonly ICommand<HostingInput> _configure;

	public ConfigureHostBuilderFromEnvironment(ICommand<HostingInput> configure)
		=> _configure = configure;

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureAppConfiguration((context, builder) => _configure.Execute(new(parameter, context, builder)));
	}
}