using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class AssignServiceLocatorCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			container.TryResolve<IServiceLocator>().NotNull( ServiceLocation.Assign );
		}
	}
}