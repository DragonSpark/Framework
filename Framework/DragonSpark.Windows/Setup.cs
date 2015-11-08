using System.Configuration;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Logging;
using DragonSpark.Setup;
using Microsoft.Practices.Unity.Configuration;

namespace DragonSpark.Windows
{
	public class Setup<TLoggingFacade> : Setup<TLoggingFacade, AssemblyModuleCatalog> where TLoggingFacade : ILoggerFacade, new()
	{}

	public class SetupUnityFromConfigurationCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			ConfigurationManager.GetSection( "unity" ).As<UnityConfigurationSection>( x => x.Containers.Any().IsTrue( () => context.Container().LoadConfiguration() ) );
		}
	}
}