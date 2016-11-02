using DragonSpark.Sources.Scopes;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public abstract class CacheWithImplementedFactoryBase<T> : CacheWithImplementedFactoryBase<object, T>, ICache<T>
	{
		protected CacheWithImplementedFactoryBase() : this( DefaultSpecification ) {}

		[UsedImplicitly]
		protected CacheWithImplementedFactoryBase( ISpecification<object> specification ) : base( specification ) {}
	}

	public abstract class CacheWithImplementedFactoryBase<TInstance, TValue> : DecoratedCache<TInstance, TValue>
	{
		readonly protected static ISpecification<TInstance> DefaultSpecification = Common<TInstance>.Assigned;

		protected CacheWithImplementedFactoryBase() : this( DefaultSpecification ) {}
		protected CacheWithImplementedFactoryBase( ISpecification<TInstance> specification ) : this( new ConfigurableParameterizedSource<TInstance, TValue>(), specification ) {}

		CacheWithImplementedFactoryBase( IConfigurableParameterizedSource<TInstance, TValue> configuration, ISpecification<TInstance> specification ) : base( configuration.Apply( specification ).ToCache() )
		{
			configuration.Configuration.Assign( new Func<TInstance, TValue>( Create ).AssignGlobal );
		}

		protected abstract TValue Create( TInstance parameter );
	}
}