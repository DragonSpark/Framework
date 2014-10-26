using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Commands
{
	public interface IContainerConfigurationCommand
	{
		void Configure( IUnityContainer container );
	}
}