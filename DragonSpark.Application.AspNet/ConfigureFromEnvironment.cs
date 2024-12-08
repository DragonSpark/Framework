using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.AspNet;

sealed class ConfigureFromEnvironment : SelectedCommand<IApplicationBuilder>
{
	public static ConfigureFromEnvironment Default { get; } = new();

	ConfigureFromEnvironment() : base(ServiceConfigurationLocator.Default.Get) {}
}