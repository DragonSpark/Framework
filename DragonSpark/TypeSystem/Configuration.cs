using DragonSpark.ComponentModel;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem.Metadata;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public static class Configuration
	{
		public static IParameterizedScope<string, ImmutableArray<string>> AssemblyPathLocator { get; } = new ParameterizedScope<string, ImmutableArray<string>>( path => Items<string>.Immutable ).ScopedWithDefault();
		public static IParameterizedScope<string, Assembly> AssemblyLoader { get; } = new ParameterizedScope<string, Assembly>( path => default(Assembly) ).ScopedWithDefault();

		public static IParameterizedScope<IEnumerable<Assembly>, Assembly> ApplicationAssemblyLocator { get; } = new ParameterizedScope<IEnumerable<Assembly>, Assembly>( Application.ApplicationAssemblyLocator.Default.Get );
		public static IScope<ImmutableArray<ITypeDefinitionProvider>> TypeDefinitionProviders { get; } = new Scope<ImmutableArray<ITypeDefinitionProvider>>( TypeDefinitionProviderSource.Default.Get );
	}
}