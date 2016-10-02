using DragonSpark.Specifications;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public abstract class FactoryCache<T> : FactoryCache<object, T>, ICache<T>
	{
		protected FactoryCache() : this( DefaultSpecification ) {}
		protected FactoryCache( ISpecification<object> specification ) : base( specification ) {}
	}

	public abstract class FactoryCache<TInstance, TValue> : DecoratedCache<TInstance, TValue>
	{
		readonly protected static ISpecification<TInstance> DefaultSpecification = Common<TInstance>.Always;

		protected FactoryCache() : this( DefaultSpecification ) {}
		protected FactoryCache( ISpecification<TInstance> specification ) : this( new AssignableParameterizedSource<TInstance, TValue>( instance => default(TValue) ), specification ) {}

		FactoryCache( IAssignableParameterizedSource<TInstance, TValue> configuration, ISpecification<TInstance> specification ) : base( configuration.ToCache() )
		{
			IParameterizedSource<TInstance, TValue> source = new DelegatedParameterizedSource<TInstance, TValue>( Create );
			var factory = specification == DefaultSpecification ? source : source.Apply( specification );
			configuration.Assign( factory.ToSourceDelegate() );
		}

		protected abstract TValue Create( TInstance parameter );
	}

	
}