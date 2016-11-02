using DragonSpark.Sources.Parameterized;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace DragonSpark.Sources.Scopes
{
	[SuppressMessage( "ReSharper", "PossibleInfiniteInheritance" )]
	public class Alterations<T> : Scope<ImmutableArray<IAlteration<T>>>, IAlterations<T>
	{
		public Alterations( params IAlteration<T>[] configurators ) : base( new AlterationsSource<T>( configurators ).GlobalCache() ) {}
	}
}