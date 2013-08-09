using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public interface IUnityContainerTypeConfiguration
	{
		void Configure( IUnityContainer container, UnityType type );
	}
}