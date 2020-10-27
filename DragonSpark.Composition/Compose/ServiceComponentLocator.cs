using DragonSpark.Runtime.Environment;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition.Compose
{
	public class ServiceComponentLocator<T> : LocateComponent<IServiceCollection, T>
	{
		protected ServiceComponentLocator() : base(x => x.GetRequiredInstance<IComponentType>()) {}
	}
}