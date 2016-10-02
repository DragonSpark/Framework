using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class WritableSourceCache<TInstance, TValue> : Cache<TInstance, IAssignableSource<TValue>>, ISourceCache<TInstance, TValue> where TInstance : class
	{
		public WritableSourceCache() : this( instance => new SuppliedSource<TValue>() ) {}

		public WritableSourceCache( Func<TInstance, TValue> create ) : this( new Func<TInstance, IAssignableSource<TValue>>( new Context( create ).Create ) ) {}

		public WritableSourceCache( Func<TInstance, IAssignableSource<TValue>> create ) : base( create ) {}

		class Context
		{
			readonly Func<TInstance, TValue> create;
			public Context( Func<TInstance, TValue> create )
			{
				this.create = create;
			}

			public IAssignableSource<TValue> Create( TInstance instance ) => new SuppliedSource<TValue>( create( instance ) );
		}
	}
}