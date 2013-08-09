using DragonSpark.Extensions;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC.Configuration
{
	public class ExecuteExportedConfigurationsCommand : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			var commands = container.Configure<CompositionExtension>().CompositionContainer.GetExportedValues<IContainerConfigurationCommand>();
			commands.Apply( x => x.Configure( container ) );
		}
	}
}