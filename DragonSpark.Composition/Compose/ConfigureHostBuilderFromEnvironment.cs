using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

public sealed class ConfigureHostBuilderFromEnvironment : ICommand<IHostBuilder>
{
	public static ConfigureHostBuilderFromEnvironment Default { get; } = new();

	ConfigureHostBuilderFromEnvironment() : this(ConfigureHostBuilderFromEnvironmentCommand.Default) {}

	readonly ICommand<(IHostEnvironment Environment, IHostBuilder Builder)> _configure;

	public ConfigureHostBuilderFromEnvironment(ICommand<(IHostEnvironment Environment, IHostBuilder Builder)> configure)
		=> _configure = configure;

	public void Execute(IHostBuilder parameter)
	{
		parameter.ConfigureAppConfiguration((context, _) => _configure.Execute(context.HostingEnvironment, parameter));
	}
}