using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Sources.Scopes
{
	public class AggregateSource<T> : DelegatedSource<T>
	{
		readonly ImmutableArray<IAlteration<T>> alterations;
		
		public AggregateSource( Func<T> seed, params IAlteration<T>[] alterations ) : base( seed )
		{
			this.alterations = alterations.ToImmutableArray();
		}

		public override T Get() => 
			alterations.Aggregate( base.Get(), ( current, transformer ) => transformer.Get( current ) );
	}
}