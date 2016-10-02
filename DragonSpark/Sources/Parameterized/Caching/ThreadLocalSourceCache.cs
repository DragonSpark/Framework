using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class ThreadLocalSourceCache<T> : ThreadLocalSourceCache<object, T>
	{
		public ThreadLocalSourceCache() {}
		public ThreadLocalSourceCache( Func<T> create ) : base( create ) {}
		public ThreadLocalSourceCache( Func<object, IAssignableSource<T>> create ) : base( create ) {}
	}

	public class ThreadLocalSourceCache<TInstance, TResult> : WritableSourceCache<TInstance, TResult> where TInstance : class
	{
		readonly static Func<TInstance, IAssignableSource<TResult>> Create = Store.Default.Get;
		public ThreadLocalSourceCache() : this( Create ) {}

		public ThreadLocalSourceCache( Func<TResult> create ) : this( new Store( create ).Get ) {}

		public ThreadLocalSourceCache( Func<TInstance, IAssignableSource<TResult>> create ) : base( create ) {}

		sealed class Store : ParameterizedSourceBase<TInstance, IAssignableSource<TResult>>
		{
			public static Store Default { get; } = new Store();

			readonly Func<TResult> create;

			Store() : this( () => default(TResult) ) {}

			public Store( Func<TResult> create )
			{
				this.create = create;
			}

			public override IAssignableSource<TResult> Get( TInstance instance ) => new ThreadLocalStore<TResult>( create );
		}
	}
}