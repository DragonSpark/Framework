using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application;

sealed class ConfigureFromEnvironment : SelectedCommand<IApplicationBuilder>
{
	public static ConfigureFromEnvironment Default { get; } = new ConfigureFromEnvironment();

	ConfigureFromEnvironment() : base(ServiceConfigurationLocator.Default.Get) {}
}