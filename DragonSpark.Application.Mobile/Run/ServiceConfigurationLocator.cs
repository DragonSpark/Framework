using DragonSpark.Composition.Compose;
using DragonSpark.Runtime.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile.Run;

sealed class ServiceConfigurationLocator : LocateComponent<Application, IApplicationConfiguration>
{
	public static ServiceConfigurationLocator Default { get; } = new();

	ServiceConfigurationLocator() : base(x => x.Host.Services.GetRequiredService<IComponentType>()) {}
}