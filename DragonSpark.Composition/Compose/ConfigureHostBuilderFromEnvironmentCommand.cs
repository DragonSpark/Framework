using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

sealed class ConfigureHostBuilderFromEnvironmentCommand : ICommand<(IHostEnvironment Environment, IHostBuilder Builder)>
{
	public static ConfigureHostBuilderFromEnvironmentCommand Default { get; } = new();

	ConfigureHostBuilderFromEnvironmentCommand()
		: this(ModularityComponents.Default.Then()
		                           .Select(x => x.ComponentType)
		                           .Select(Locate<IHostConfiguration>.Default)
		                           .Get()) {}

	readonly ISelect<IHostEnvironment, IHostConfiguration> _configuration;

	public ConfigureHostBuilderFromEnvironmentCommand(ISelect<IHostEnvironment, IHostConfiguration> configuration)
		=> _configuration = configuration;

	public void Execute((IHostEnvironment Environment, IHostBuilder Builder) parameter)
	{
		var (environment, builder) = parameter;
		_configuration.Get(environment).Execute(builder);
	}
}