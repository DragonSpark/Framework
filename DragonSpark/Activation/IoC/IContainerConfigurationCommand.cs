using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	public interface IContainerConfigurationCommand
	{
		void Configure( IUnityContainer container );
	}
}