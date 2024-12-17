using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Run;

sealed class ConfigureFromEnvironment : SelectedCommand<Application>
{
	public static ConfigureFromEnvironment Default { get; } = new();

	ConfigureFromEnvironment() : base(ServiceConfigurationLocator.Default.Get) {}
}