using DragonSpark.Application;
using DragonSpark.Application.Setup;
using DragonSpark.ComponentModel;
using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using DragonSpark.Windows.Runtime;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Reflection;

namespace DragonSpark.Windows
{
	[Export( typeof(ISetup) )]
	public class InitializationCommand : DeclarativeSetup
	{
		readonly static ImmutableArray<ITypeDefinitionProvider> Providers = TypeDefinitions.Source.Implementation.Get().Insert( 0, MetadataTypeDefinitionProvider.Default );
		readonly static Func<Assembly, bool> Specification =
			new DelegatedSpecification<Assembly>( ApplicationAssemblySpecification.Default.GetFactory() )
				.Or( DomainAssemblySpecification.Default )
				.IsSatisfiedBy;

		public InitializationCommand() : base( Priority.Higher,
			TypeDefinitions.Source.Implementation.ToCommand( Providers ),
			ApplicationAssemblySpecification.Default.ToCommand( Specification )
		) {}
	}
}