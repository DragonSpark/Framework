using DragonSpark.Setup.Commands;
using Microsoft.Practices.Unity;

namespace DragonSpark.Setup.Registration
{
	public interface IUnityContainerTypeConfiguration
	{
		void Configure( IUnityContainer container, UnityType type );
	}
}