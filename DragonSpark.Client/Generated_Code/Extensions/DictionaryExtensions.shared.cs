using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Extensions
{
	public static class DictionaryExtensions
	{
		public static TValue TryGet<TKey,TValue>( this IDictionary<TKey,TValue> target, TKey key, Func<TValue> defaultValue = null )
		{
			var result = target.ContainsKey( key ) ? target[ key ] : defaultValue.Transform( x => x() );
			return result;
		}

		public static void ExecuteOn<TKey, TValue>( this IDictionary<TKey, TValue> target, TKey key, Action<TValue> action )
		{
			target.ContainsKey( key ).IsTrue( () => action( target[key] ) );
		}

		[MethodImpl( MethodImplOptions.Synchronized )]
		public static TValue Ensure<TKey, TValue>( this IDictionary<TKey, TValue> target, TKey key, Func<TKey,TValue> resolve = null )
		{
			Contract.Requires( target != null );
			// Contract.Requires( !key.IsDefault() );

			resolve = resolve ?? ( item => ServiceLocator.Current.GetInstance<TValue>() );

			if ( !target.ContainsKey( key ) )
			{
				target.Add( key, resolve( key ) );
			}
			return target[ key ];
		}
	}
}