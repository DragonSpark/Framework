using DragonSpark.Composition.Compose;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Application.Mobile.Maui.Run;

sealed class ServiceConfigurationLocator : LocateComponent<MauiApp, IApplicationConfiguration>
{
	public static ServiceConfigurationLocator Default { get; } = new();

	ServiceConfigurationLocator() : base(x => x.Services.GetRequiredService<IComponentType>()) {}
}