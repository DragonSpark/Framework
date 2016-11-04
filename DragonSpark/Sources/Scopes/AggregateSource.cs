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

	/*public class AggregateScope<T> : Scope<T>
	{
				public AggregateScope( params IAlteration<T>[] alterations ) : this( DefaultSeed, alterations ) {}

		public AggregateScope( Func<T> seed, params IAlteration<T>[] alterations ) : this( seed.ToScope(), new SingletonScope<IAlterations<T>>( new Alterations<T>( alterations ) ) ) {}

		public AggregateScope( IScope<T> seed, IScope<IAlterations<T>> alterations ) : base( new AggregateSource<T>( seed.Get, alterations.GetValue ).Get )
		{
			Seed = seed;
			Alterations = alterations;
		}

		
	}*/
}