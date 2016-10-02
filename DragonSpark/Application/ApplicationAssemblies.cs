using System.Collections.Immutable;
using System.Reflection;
using DragonSpark.Sources;

namespace DragonSpark.Application
{
	public sealed class ApplicationAssemblies : DelegatedSource<ImmutableArray<Assembly>>
	{
		public static ISource<ImmutableArray<Assembly>> Default { get; } = new ApplicationAssemblies();
		ApplicationAssemblies() : base( () => ApplicationParts.Default.Get().Assemblies ) {}
	}
}