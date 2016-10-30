using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System;
using System.Reflection;

namespace DragonSpark.Sources
{
	public static class Scopes
	{
		public static IScope<T> CreateScope<T>( this IScope<T> @this ) => @this.ToDelegate().CreateScope();
		public static IScope<T> CreateScope<T>( this Func<T> @this ) => new Scope<T>( @this.GlobalCache() );

		public static IParameterizedScope<TParameter, TResult> CreateScope<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToSourceDelegate().CreateScope();
		public static IParameterizedScope<TParameter, TResult> CreateScope<TParameter, TResult>( this Func<TParameter, TResult> @this ) => new ParameterizedScope<TParameter, TResult>( @this.GlobalCache() );

		public static IScope<T> ToScope<T>( this IParameterizedSource<object, T> @this ) => @this.ToSourceDelegate().ToScope();
		public static IScope<T> ToScope<T>( this Func<object, T> @this ) => ScopeCaches<T>.Default.Get( @this );

		sealed class ScopeCaches<T> : Cache<Func<object, T>, IScope<T>>
		{
			public static ScopeCaches<T> Default { get; } = new ScopeCaches<T>();
			ScopeCaches() : base( cache => new Scope<T>( cache.Cache<object, T>() ) ) {}
		}
	}

	public static class Factory
	{
		public static T Self<T>( this T @this ) => @this;

		public static Func<T> For<T>( T @this ) => ( typeof(T).GetTypeInfo().IsValueType ? new Source<T>( @this ) : @this.Sourced() ).Get;

		public static Func<T> ToCachedDelegate<T>( this ISource<T> @this ) => new Func<T>( @this.Get ).Cache();
		public static Func<T> Cache<T>( this Func<T> @this ) => CachedFactoryBuilder<T>.Default.Get( @this );
		public static Func<TParameter, TResult> Cache<TParameter, TResult>( this Func<TParameter, TResult> @this ) => CacheFactory.Create( @this ).Get;

		public static Func<object, T> GlobalCache<T>( this ISource<T> @this ) => @this.ToDelegate().GlobalCache();
		public static Func<object, T> GlobalCache<T>( this Func<T> @this ) => @this.Wrap().Cache();
		public static Func<object, Func<TParameter, TResult>> GlobalCache<TParameter, TResult>( this IParameterizedSource<TParameter, TResult> @this ) => @this.ToSourceDelegate().GlobalCache();
		public static Func<object, Func<TParameter, TResult>> GlobalCache<TParameter, TResult>( this Func<TParameter, TResult> @this ) => new Caches<TParameter, TResult>( @this ).Get;

		sealed class Caches<TParameter, TResult> : FactoryCache<Func<TParameter, TResult>>
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