using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class EqualityReferenceCache<TInstance, TValue> : DecoratedCache<TInstance, TValue> where TInstance : class
	{
		readonly static Alter<TInstance> DefaultSource = EqualityReference<TInstance>.Default.Get;

		readonly Alter<TInstance> equalitySource;

		public EqualityReferenceCache() : this( instance => default(TValue) ) {}
		public EqualityReferenceCache( Func<TInstance, TValue> create ) : this( create, DefaultSource ) {}
		public EqualityReferenceCache( Func<TInstance, TValue> create , Alter<TInstance> equalitySource ) : this( CacheFactory.Create( create ), equalitySource ) {}

		public EqualityReferenceCache( ICache<TInstance, TValue> inner, Alter<TInstance> equalitySource ) : base( inner )
		{
			this.equalitySource = equalitySource;
		}

		public override bool Contains( TInstance instance ) => base.Contains( equalitySource( instance ) );

		public override bool Remove( TInstance instance ) => base.Remove( equalitySource( instance ) );

		public override void Set( TInstance instance, TValue value ) => base.Set( equalitySource( instance ), value );

		public override TValue Get( TInstance parameter ) => base.Get( equalitySource( parameter )  );
	}
}