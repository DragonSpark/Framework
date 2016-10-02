using System;
using System.Runtime.CompilerServices;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public class Cache<T> : Cache<object, T>, ICache<T> where T : class
	{
		public Cache() {}
		public Cache( Func<object, T> create ) : base( create ) {}
	}

	public class Cache<TInstance, TValue> : CacheBase<TInstance, TValue>, IAtomicCache<TInstance, TValue> where TInstance : class where TValue : class
	{
		readonly ConditionalWeakTable<TInstance, TValue>.CreateValueCallback create;

		readonly ConditionalWeakTable<TInstance, TValue> items = new ConditionalWeakTable<TInstance, TValue>();

		public Cache() : this( new Func<TInstance, TValue>( instance => default(TValue) ) ) {}

		public Cache( Func<TInstance, TValue> create ) : this( new ConditionalWeakTable<TInstance, TValue>.CreateValueCallback( create ) ) {}

		Cache( ConditionalWeakTable<TInstance, TValue>.CreateValueCallback create )
		{
			this.create = create;
		}

		public override bool Contains( TInstance instance )
		{
			TValue temp;
			return items.TryGetValue( instance, out temp );
		}

		public override void Set( TInstance instance, TValue value )
		{
			lock ( items )
			{
				items.Remove( instance );
				items.Add( instance, value );
			}
		}

		public override TValue Get( TInstance parameter ) => items.GetValue( parameter, create );
		
		public override bool Remove( TInstance instance ) => items.Remove( instance );

		public TValue GetOrSet( TInstance key, Func<TInstance, TValue> factory ) => items.GetValue( key, new ConditionalWeakTable<TInstance, TValue>.CreateValueCallback( factory ) );
	}
}