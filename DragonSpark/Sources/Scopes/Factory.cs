using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Reflection;

namespace DragonSpark.Sources.Scopes
{
	public static class Factory
	{
		public static T Self<T>( this T @this ) => @this;

		public static Func<T> For<T>( T @this ) => ( typeof(T).GetTypeInfo().IsValueType ? new Source<T>( @this ) : @this.Sourced() ).Get;
		
		public static Func<T> ToSingleton<T>( this ISource<T> @this ) => new Func<T>( @this.Get ).ToSingleton();
		public static Func<T> ToSingleton<T>( this Func<T> @this ) => SingletonDelegateBuilder<T>.Default.Get( @this );
		public static Func<TParameter, TResult> ToSingleton<TParameter, TResult>( this Func<TParameter, TResult> @this ) => Caches.Create( @this ).Get;

		public static Func<object, T> ToGlobalSingleton<T>( this ISource<T> @this ) => @this.ToDelegate().ToGlobalSingleton();
		public static Func<object, T> ToGlobalSingleton<T>( this Func<T> @this ) => Caches.Create( @this ).Get;
		public static Func<object, Func<TParameter, TResult>> ToGlobalSingleton<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToDelegate().ToGlobalSingleton();
		public static Func<object, Func<TParameter, TResult>> ToGlobalSingleton<TParameter, TResult>( this Func<TParameter, TResult> @this ) => ToGlobalSingleton( @this.ToSingleton );
		public static Func<object, Func<TParameter, TResult>> ToGlobalSingleton<TParameter, TResult>( this Func<Func<TParameter, TResult>> @this ) => Caches.Create( @this ).Get;
	}
}