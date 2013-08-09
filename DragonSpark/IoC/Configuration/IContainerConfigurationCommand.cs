using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public interface IContainerConfigurationCommand
	{
		void Configure( IUnityContainer container );
	}
}