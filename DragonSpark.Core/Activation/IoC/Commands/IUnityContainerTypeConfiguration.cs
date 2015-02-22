using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC.Commands
{
	public interface IUnityContainerTypeConfiguration
	{
		void Configure( IUnityContainer container, UnityType type );
	}
}