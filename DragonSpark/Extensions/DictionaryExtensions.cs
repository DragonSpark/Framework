using DragonSpark.Runtime.Assignments;
using System;
using System.Collections.Generic;

namespace DragonSpark.Extensions
{
	public static class DictionaryExtensions
	{
		public static TValue TryGet<TKey,TValue>( this IDictionary<TKey,TValue> target, TKey key ) => TryGet( target, key, () => default(TValue) );

		public static TValue TryGet<TKey,TValue>( this IDictionary<TKey,TValue> target, TKey key, Func<TValue> defaultValue ) => key.IsAssigned() && target.ContainsKey( key ) ? target[ key ] : defaultValue.With( x => x() );

		// public static void ExecuteOn<TKey, TValue>( this IDictionary<TKey, TValue> target, TKey key, Action<TValue> action ) => target.ContainsKey( key ).IsTrue( () => action( target[key] ) );

		public static TValue Ensure<TKey, TValue>( this IDictionary<TKey, TValue> target, TKey key, Func<TKey,TValue> resolve )
		{
			if ( !target.ContainsKey( key ) )
			{
				target.Add( key, resolve( key ) );
			}
			return target[ key ];
		}

		public static Assignment<T1, T2> Assignment<T1, T2>( this IDictionary<T1, T2> @this, T1 first, T2 second )  => new Assignment<T1, T2>( new DictionaryAssign<T1, T2>( @this ), Assignments.From( first ), new Value<T2>( second ) );
	}
}