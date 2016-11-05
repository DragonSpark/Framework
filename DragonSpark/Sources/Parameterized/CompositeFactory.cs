using DragonSpark.Extensions;
using System.Collections.Immutable;

namespace DragonSpark.Sources.Parameterized
{
	public class CompositeFactory<TParameter, TResult> : DecoratedParameterizedSource<TParameter, TResult>
	{
		public CompositeFactory( params IParameterizedSource<TParameter, TResult>[] sources ) : this( sources.ToImmutableArray() ) {}
		CompositeFactory( ImmutableArray<IParameterizedSource<TParameter, TResult>> sources ) : base( new Inner( sources ) ) {}

		sealed class Inner : ParameterizedSourceBase<TParameter, TResult>
		{
			readonly static TResult DefaultResult = default(TResult);

			readonly ImmutableArray<IParameterizedSource<TParameter, TResult>> sources;

			public Inner( ImmutableArray<IParameterizedSource<TParameter, TResult>> sources )
			{
				this.sources = sources;
			}

			public override TResult Get( TParameter parameter )
			{
				foreach ( var source in sources )
				{
					var candidate = source.Get( parameter );
					if ( candidate.IsAssigned() )
					{
						return candidate;
					}
				}
				return DefaultResult;
			}
		}
	}
}