using DragonSpark.Model.Commands;
using Microsoft.Maui.Hosting;

namespace DragonSpark.Application.Mobile.Maui.Run;

sealed class ConfigureFromEnvironment : SelectedCommand<MauiApp>
{
	public static ConfigureFromEnvironment Default { get; } = new();

	ConfigureFromEnvironment() : base(ServiceConfigurationLocator.Default.Get) {}
}