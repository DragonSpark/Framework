using JetBrains.Annotations;
using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class EqualityReferenceCache<TInstance, TValue> : AlteredCache<TInstance, TValue> where TInstance : class
	{
		readonly static Alter<TInstance> DefaultSource = EqualityReference<TInstance>.Default.Get;

		public EqualityReferenceCache() : this( instance => default(TValue) ) {}

		public EqualityReferenceCache( Func<TInstance, TValue> create ) : this( create, DefaultSource ) {}

		[UsedImplicitly]
		public EqualityReferenceCache( Func<TInstance, TValue> create , Alter<TInstance> equalitySource ) : this( CacheFactory.Create( create ), equalitySource ) {}

		[UsedImplicitly]
		public EqualityReferenceCache( ICache<TInstance, TValue> inner, Alter<TInstance> equalitySource ) : base( inner, equalitySource ) {}
	}
}