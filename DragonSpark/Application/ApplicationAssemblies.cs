using DragonSpark.Sources;
using DragonSpark.TypeSystem;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Application
{
	public sealed class ApplicationAssemblies : DelegatedSource<ImmutableArray<Assembly>>
	{
		public static ISource<ImmutableArray<Assembly>> Default { get; } = new ApplicationAssemblies();
		ApplicationAssemblies() : base( () => ApplicationParts.Default.Get()?.Assemblies ?? Items<Assembly>.Immutable ) {}
	}
}