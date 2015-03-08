using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC
{
	public partial class CompositionConfiguration : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			var composition = container.Configure<CompositionExtension>().CompositionContainer;
			container.RegisterInstance( composition );
			OnConfigure( container );
		}

		partial void OnConfigure( IUnityContainer container );
	}
}
