using DragonSpark.Composition;
using DragonSpark.Sources;
using System.Collections.Immutable;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Application
{
	public sealed class SingletonExports : Scope<ImmutableArray<SingletonExport>>
	{
		public static ISource<ImmutableArray<SingletonExport>> Default { get; } = new SingletonExports();
		SingletonExports() : base( Factory.GlobalCache( () => ExportsProfileFactory.Default.Get().Singletons.ToImmutableArray() ) ) {}
	}
}