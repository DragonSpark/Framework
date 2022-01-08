using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose;

sealed class ConfigureFromEnvironment : SelectedCommand<IServiceCollection>
{
	public static ConfigureFromEnvironment Default { get; } = new();

	ConfigureFromEnvironment() : base(EnvironmentalServiceConfiguration.Default) {}
}