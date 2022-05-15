using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

sealed class ConfigureHostBuilderFromEnvironmentCommand : ICommand<HostingInput>
{
	public static ConfigureHostBuilderFromEnvironmentCommand Default { get; } = new();

	ConfigureHostBuilderFromEnvironmentCommand()
		: this(ModularityComponents.Default.Then()
		                           .Select(x => x.ComponentType)
		                           .Select(Locate<IHostConfiguration>.Default)
		                           .Get()) {}

	readonly ISelect<HostBuilderContext, IHostConfiguration> _configuration;

	public ConfigureHostBuilderFromEnvironmentCommand(ISelect<HostBuilderContext, IHostConfiguration> configuration)
		=> _configuration = configuration;

	public void Execute(HostingInput parameter)
	{
		_configuration.Get(parameter.Context).Execute(parameter);
	}
}