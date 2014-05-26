using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Configuration
{
	public class ApplicationConfigurationCommand : IContainerConfigurationCommand
	{
		public ApplicationDetails ApplicationDetails { get; set; }

		public void Configure( IUnityContainer container )
		{
			OnConfigure( container );
		}

		protected virtual void OnConfigure( IUnityContainer container )
		{
			container.RegisterInstance( ApplicationDetails );
		}
	}
}