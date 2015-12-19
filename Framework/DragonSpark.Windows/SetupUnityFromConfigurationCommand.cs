using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Setup.Commands;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Linq;

namespace DragonSpark.Windows
{
	public class SetupUnityFromConfigurationCommand : UnityCommand
	{
		protected override void Execute( SetupContext context )
		{
			ConfigurationManager.GetSection( "unity" ).As<UnityConfigurationSection>( x => x.Containers.Any().IsTrue( () => Container.LoadConfiguration() ) );
		}
	}
}