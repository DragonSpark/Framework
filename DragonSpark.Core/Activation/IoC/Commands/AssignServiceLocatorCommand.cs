using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC.Commands
{
	public class AssignServiceLocatorCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			container.With<IServiceLocator>( ServiceLocation.Assign );
		}
	}
}