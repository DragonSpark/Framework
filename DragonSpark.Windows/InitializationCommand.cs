using DragonSpark.Application;
using DragonSpark.Application.Setup;
using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using DragonSpark.Windows.Runtime;
using System;
using System.Composition;
using System.Reflection;

namespace DragonSpark.Windows
{
	[Export( typeof(ISetup) )]
	public class InitializationCommand : DeclarativeSetup
	{
		readonly static Func<Assembly, bool> Specification =
			new DelegatedSpecification<Assembly>( ApplicationAssemblySpecification.Default.Configuration.GetFactory() )
				.Or( DomainAssemblySpecification.Default )
				.IsSatisfiedBy;

		public InitializationCommand() : base( Priority.Higher,
			TypeSystem.Configuration.TypeDefinitionProviders.ToCommand( TypeDefinitionProviderSource.Default.ToCachedDelegate() ),
			ApplicationAssemblySpecification.Default.Configuration.ToCommand( Specification )
		) {}
	}
}