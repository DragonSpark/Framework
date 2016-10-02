using DragonSpark.Application.Setup;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.Runtime;
using System.Composition;

namespace DragonSpark.Windows
{
	[Export( typeof(ISetup) )]
	public class InitializationCommand : DeclarativeSetup
	{
		public InitializationCommand() : base( Priority.Higher,
			TypeSystem.Configuration.TypeDefinitionProviders.Configured( TypeDefinitionProviderSource.Default.ToCachedDelegate() ),
			TypeSystem.Configuration.ApplicationAssemblyLocator.Configured( ApplicationAssemblyLocator.Default.ToSourceDelegate() )
		)
		{}
	}
}