using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System;
using System.Collections.Immutable;
using System.Linq;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Sources.Scopes
{
	public class AggregateSource<T> : DelegatedSource<T>
	{
		readonly static Func<T> DefaultSeed = Activator.Default.Get<T>;

		readonly ImmutableArray<IAlteration<T>> alterations;

		public AggregateSource( params IAlteration<T>[] alterations ) : this( DefaultSeed, alterations ) {}
		public AggregateSource( Func<T> seed, params IAlteration<T>[] alterations ) : base( seed )
		{
			this.alterations = alterations.ToImmutableArray();
		}

		public override T Get() => 
			alterations.Aggregate( base.Get(), ( current, transformer ) => transformer.Get( current ) );
	}
}