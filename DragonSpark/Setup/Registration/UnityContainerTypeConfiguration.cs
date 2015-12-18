using DragonSpark.Setup.Commands;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
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