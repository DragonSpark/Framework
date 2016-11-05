using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace DragonSpark.Sources.Scopes
{
	public class AggregateParameterizedSource<T> : AggregateParameterizedSource<object, T>
	{
		public AggregateParameterizedSource( Func<object, T> seed, IItemSource<IAlteration<T>> alterations ) : base( seed, alterations ) {}
	}

	public class AggregateParameterizedSource<TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>
	{
		readonly IItemSource<IAlteration<TResult>> alterations;

		[UsedImplicitly]
		public AggregateParameterizedSource( Func<TParameter, TResult> seed, IItemSource<IAlteration<TResult>> alterations ) : base( seed )
		{
			this.alterations = alterations;
		}

		public override TResult Get( TParameter parameter ) => 
			alterations.Aggregate( base.Get( parameter ), ( current, transformer ) => transformer.Get( current ) );
	}
}