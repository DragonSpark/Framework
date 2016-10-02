using DragonSpark.Extensions;
using DragonSpark.Runtime.Assignments;
using System;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public static class CacheExtensions
	{
		public static TValue GetAssigned<TInstance, TValue>( this ICache<TInstance, TValue> @this, TInstance instance )
		{
			var result = @this.Get( instance );
			if ( !result.IsAssigned() )
			{
				@this.Remove( instance );
			}
			return result;
		}

		public static TValue SetValue<TInstance, TValue>( this IAssignableReferenceSource<TInstance, TValue> @this, TInstance instance, TValue value )
		{
			@this.Set( instance, value );
			return value;
		}

		public static TValue SetOrClear<TInstance, TValue>( this ICache<TInstance, TValue> @this, TInstance instance, TValue value = default(TValue) )
		{
			if ( value.IsAssigned() )
			{
				@this.Set( instance, value );
			}
			else
			{
				@this.Remove( instance );
			}
			
			return value;
		}

		public static Assignment<T1, T2> Assignment<T1, T2>( this ICache<T1, T2> @this, T1 instance, T2 start, T2 finish = default(T2) ) => new Assignment<T1, T2>( new CacheAssign<T1, T2>( @this ), Assignments.From( instance ), new Value<T2>( start, finish ) );

		public static Func<TInstance, TValue> ToDelegate<TInstance, TValue>( this ICache<TInstance, TValue> @this ) => Delegates<TInstance, TValue>.Default.Get( @this );
		sealed class Delegates<TInstance, TValue> : Cache<ICache<TInstance, TValue>, Func<TInstance, TValue>>
		{
			public static Delegates<TInstance, TValue> Default { get; } = new Delegates<TInstance, TValue>();
			Delegates() : base( command => command.Get ) {}
		}
	}
}