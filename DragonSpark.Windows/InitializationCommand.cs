using DragonSpark.Application.Setup;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Scopes;
using DragonSpark.Windows.Runtime;
using System.Composition;

namespace DragonSpark.Windows
{
	[Export( typeof(ISetup) )]
	public class InitializationCommand : DeclarativeSetup
	{
		public InitializationCommand() : base( Priority.Higher,
			TypeSystem.Configuration.TypeDefinitionProviders.ToCommand( TypeDefinitionProviderSource.Default.ToCachedDelegate() ),
			TypeSystem.Configuration.ApplicationAssemblyLocator.ToCommand( ApplicationAssemblyLocator.Default.ToDelegate() )
		)
		{}
	}
}