using DragonSpark.Composition;
using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using System.Collections.Immutable;

namespace DragonSpark.Application
{
	public sealed class SingletonExports : ScopedSingleton<ImmutableArray<SingletonExport>>
	{
		public static ISource<ImmutableArray<SingletonExport>> Default { get; } = new SingletonExports();
		SingletonExports() : base( () => ExportsProfileFactory.Default.Get().Singletons.ToImmutableArray() ) {}
	}
}