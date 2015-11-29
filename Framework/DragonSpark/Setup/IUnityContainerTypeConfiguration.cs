using Microsoft.Practices.Unity;

namespace DragonSpark.Setup
{
	public interface IUnityContainerTypeConfiguration
	{
		void Configure( IUnityContainer container, UnityType type );
	}
}