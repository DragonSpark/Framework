using System.Configuration;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using Microsoft.Practices.Unity.Configuration;

namespace DragonSpark.Windows
{
	public class SetupUnityFromConfigurationCommand : SetupCommand
	{
		protected override void Execute( SetupContext context )
		{
			ConfigurationManager.GetSection( "unity" ).As<UnityConfigurationSection>( x => Enumerable.Any<ContainerElement>( x.Containers ).IsTrue( () => context.Container().LoadConfiguration() ) );
		}
	}
}