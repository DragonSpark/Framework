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

		public static Func<T> ToCachedDelegate<T>( this ISource<T> @this ) => new Func<T>( @this.Get ).Cache();
		public static Func<T> Cache<T>( this Func<T> @this ) => CachedFactoryBuilder<T>.Default.Get( @this );
		public static Func<TParameter, TResult> Cache<TParameter, TResult>( this Func<TParameter, TResult> @this ) => CacheFactory.Create( @this ).Get;

		public static T Global<T>( this ISource<ISource<T>> @this, object _ ) => @this.GetValue();
		/*public static Func<object, T> GlobalCache<T>( this ISource<ISource<T>> @this ) => @this.ToVDelegate().GlobalCache();*/
		public static Func<object, T> Singleton<T>( this ISource<T> @this ) => @this.ToDelegate().Singleton();
		public static Func<object, T> Singleton<T>( this Func<T> @this ) => @this.Wrap().Cache();
		public static Func<object, Func<TParameter, TResult>> Singleton<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToDelegate().Singleton();
		public static Func<object, Func<TParameter, TResult>> Singleton<TParameter, TResult>( this Func<TParameter, TResult> @this ) => new Caches<TParameter, TResult>( @this ).Get;
		public static Func<object, Func<TParameter, TResult>> Singleton<TParameter, TResult>( this Func<Func<TParameter, TResult>> @this ) => CacheFactory.Create( @this ).Get;
		sealed class Caches<TParameter, TResult> : CacheWithImplementedFactoryBase<Func<TParameter, TResult>>
		{
			readonly Func<TParameter, TResult> factory;

			public Caches( Func<TParameter, TResult> factory )
			{
				this.factory = factory;
			}

			protected override Func<TParameter, TResult> Create( object parameter ) => CacheFactory.Create( factory ).Get;
		}
	}
}