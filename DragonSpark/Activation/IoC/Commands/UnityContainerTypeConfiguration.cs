using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC.Commands
{
	public class UnityContainerTypeConfiguration : IUnityContainerTypeConfiguration
	{
		protected virtual void Configure( IUnityContainer container, UnityType type )
		{}

		void IUnityContainerTypeConfiguration.Configure( IUnityContainer container, UnityType type )
		{
			Configure( container, type );
		}
	}
}