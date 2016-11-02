using DragonSpark.ComponentModel;
using DragonSpark.Sources.Scopes;
using DragonSpark.TypeSystem.Metadata;
using System.Collections.Immutable;

namespace DragonSpark.TypeSystem
{
	public static class Configuration
	{
		/*public static IParameterizedScope<IEnumerable<Assembly>, Assembly> ApplicationAssemblyLocator { get; } = new ParameterizedScope<IEnumerable<Assembly>, Assembly>( Application.ApplicationAssemblyLocator.Default.Get );*/
		public static IScope<ImmutableArray<ITypeDefinitionProvider>> TypeDefinitionProviders { get; } = new Scope<ImmutableArray<ITypeDefinitionProvider>>( TypeDefinitionProviderSource.Default.Get );
	}
}