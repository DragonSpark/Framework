using DragonSpark.Activation.IoC;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Client.Threading
{
	public class InitializeThreadingCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			container.Resolve<IDispatchHandler>();
		}
	}
}