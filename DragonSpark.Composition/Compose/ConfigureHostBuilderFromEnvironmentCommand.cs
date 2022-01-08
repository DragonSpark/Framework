using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

sealed class ConfigureHostBuilderFromEnvironmentCommand : ICommand<HostConfiguration>
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

	public void Execute(HostConfiguration parameter)
	{
		_configuration.Get(parameter.Context.HostingEnvironment).Execute(parameter);
	}
}