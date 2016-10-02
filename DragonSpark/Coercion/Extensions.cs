using DragonSpark.Sources.Parameterized.Caching;
using System;

namespace DragonSpark.Coercion
{
	public static class Extensions
	{
		public static Func<TFrom, TTo> ToDelegate<TFrom, TTo>( this ICoercer<TFrom, TTo> @this ) => Delegates<TFrom, TTo>.Default.Get( @this );
		class Delegates<TFrom, TTo> : Cache<ICoercer<TFrom, TTo>, Func<TFrom, TTo>>
		{
			public static Delegates<TFrom, TTo> Default { get; } = new Delegates<TFrom, TTo>();
			Delegates() : base( command => command.Coerce ) {}
		}

		public static Func<object, T> ToDelegate<T>( this ICoercer<T> @this ) => Coercers<T>.Default.Get( @this );
		class Coercers<T> : Cache<ICoercer<T>, Func<object, T>>
		{
			public static Coercers<T> Default { get; } = new Coercers<T>();
			Coercers() : base( command => command.Coerce ) {}
		}
	}
}