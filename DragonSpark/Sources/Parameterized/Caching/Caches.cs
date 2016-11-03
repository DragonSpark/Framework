using DragonSpark.Activation;
using System;
using System.Reflection;

namespace DragonSpark.Sources.Parameterized.Caching
{
	public static class Caches
	{
		public static ICache<T> Create<T>( Func<T> parameter ) => Create( parameter.Wrap() );

		public static ICache<T> Create<T>( Func<object, T> parameter ) => Implementations<T>.Factory( parameter );

		public static ICache<TInstance, TValue> Create<TInstance, TValue>( Func<TInstance, TValue> parameter ) => Implementations<TInstance, TValue>.Factory( parameter );

		static class Implementations<T>
		{
			public static Func<Func<object, T>, ICache<T>> Factory { get; } = Create();

			static Func<Func<object, T>, ICache<T>> Create()
			{
				var definition = typeof(T).GetTypeInfo().IsValueType ? typeof(DecoratedSourceCache<>) : typeof(Cache<>);
				var generic = definition.MakeGenericType( typeof(T) );
				var result = ParameterConstructor<Func<object, T>, ICache<T>>.Make( typeof(Func<object, T>), generic );
				return result;
			}
		}

		static class Implementations<TInstance, TValue>
		{
			public static Func<Func<TInstance, TValue>, ICache<TInstance, TValue>> Factory { get; } = Create();

			static Func<Func<TInstance, TValue>, ICache<TInstance, TValue>> Create()
			{
				var definition = typeof(TValue).GetTypeInfo().IsValueType ? typeof(DecoratedSourceCache<,>) : typeof(Cache<,>);
				var generic = definition.MakeGenericType( typeof(TInstance), typeof(TValue) );
				var result = ParameterConstructor<Func<TInstance, TValue>, ICache<TInstance, TValue>>.Make( typeof(Func<TInstance, TValue>), generic );
				return result;
			}
		}
	}
}