using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace DragonSpark.Sources.Scopes
{
	public class AggregateParameterizedSource<T> : AggregateParameterizedSource<object, T>
	{
		/*public AggregateParameterizedSource( Func<object, T> seed, params IAlteration<T>[] alterations ) : base( seed, alterations ) {}*/
		public AggregateParameterizedSource( Func<object, T> seed, IItemSource<IAlteration<T>> alterations ) : base( seed, alterations ) {}
	}

	public class AggregateParameterizedSource<TParameter, TResult> : DelegatedParameterizedSource<TParameter, TResult>
	{
		readonly IItemSource<IAlteration<TResult>> alterations;
		
		/*public AggregateParameterizedSource( Func<TParameter, TResult> seed, params IAlteration<TResult>[] alterations ) : this( seed, alterations.ToImmutableArray() ) {}*/

		[UsedImplicitly]
		public AggregateParameterizedSource( Func<TParameter, TResult> seed, IItemSource<IAlteration<TResult>> alterations ) : base( seed )
		{
			this.alterations = alterations;
		}

		public override TResult Get( TParameter parameter ) => 
			alterations.Aggregate( base.Get( parameter ), ( current, transformer ) => transformer.Get( current ) );
	}

	/*public class AggregateParameterizedScope<T> : AggregateParameterizedScope<object, T>
	{
		public AggregateParameterizedScope() {}
		public AggregateParameterizedScope( params IAlteration<T>[] alterations ) : base( alterations ) {}
		public AggregateParameterizedScope( Func<object, T> seed, params IAlteration<T>[] alterations ) : base( seed, alterations ) {}
		public AggregateParameterizedScope( IParameterizedScope<object, T> seed, IParameterizedScope<object, IAlterations<T>> alterations ) : base( seed, alterations ) {}
	}

	public class AggregateParameterizedScope<TParameter, TResult> : ParameterizedScope<TParameter, TResult>
	{
		readonly static Func<TParameter, TResult> DefaultSeed = new Func<TResult>( Activator.Default.Get<TResult> ).Wrap<TParameter, TResult>();

		public AggregateParameterizedScope() : this( Items<IAlteration<TResult>>.Default ) {}
		public AggregateParameterizedScope( params IAlteration<TResult>[] alterations ) : this( DefaultSeed, alterations ) {}

		public AggregateParameterizedScope( Func<TParameter, TResult> seed, params IAlteration<TResult>[] alterations ) 
			: this( seed.ToScope(), new ParameterizedSingletonScope<TParameter, IAlterations<TResult>>( new Alterations<TResult>( alterations ) ) ) {}

		public AggregateParameterizedScope( IParameterizedScope<TParameter, TResult> seed, IParameterizedScope<TParameter, IAlterations<TResult>> alterations ) 
			: base( new AggregateParameterizedSource<TParameter, TResult>( seed.Get, alterations.GetValue ).Get )
		{
			Seed = seed;
			Alterations = alterations;
		}

		[UsedImplicitly]
		public IParameterizedScope<TParameter, TResult> Seed { get; }

		[UsedImplicitly]
		public IParameterizedScope<TParameter, IAlterations<TResult>> Alterations { get; }
	}*/
}