using System;
using System.Collections.Generic;

namespace DragonSpark.Extensions
{
	public static class DictionaryExtensions
	{
		public static TValue TryGet<TKey,TValue>( this IDictionary<TKey,TValue> target, TKey key, Func<TValue> defaultValue = null )
		{
			var result = !Equals( default(TKey), key  ) && target.ContainsKey( key ) ? target[ key ] : defaultValue.With( x => x() );
			return result;
		}

		public static void ExecuteOn<TKey, TValue>( this IDictionary<TKey, TValue> target, TKey key, Action<TValue> action )
		{
			target.ContainsKey( key ).IsTrue( () => action( target[key] ) );
		}

		public static TValue Ensure<TKey, TValue>( this IDictionary<TKey, TValue> target, TKey key, Func<TKey,TValue> resolve )
		{
			lock ( Locker )
			{
				if ( !target.ContainsKey( key ) )
				{
					target.Add( key, resolve( key ) );
				}
			}
			return target[ key ];
		}	static readonly object Locker = new object();
	}
}