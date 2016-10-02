using DragonSpark.Expressions;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized.Caching;
using DragonSpark.TypeSystem.Generics;
using System;

namespace DragonSpark.Sources
{
	public static class Source
	{
		static IGenericMethodContext<Invoke> Methods { get; } = typeof(Source).Adapt().GenericFactoryMethods[nameof(Empty)];

		public static ISource Empty( Type type ) => Methods.Make( type ).Invoke<ISource>();

		public static ISource<T> Empty<T>() => EmptySource<T>.Default;

		public static Source<T> Sourced<T>( this T @this ) => Support<T>.Sources.Get( @this );

		static class Support<T>
		{
			public static ICache<T, Source<T>> Sources { get; } = CacheFactory.Create<T, Source<T>>( arg => new Source<T>( arg ) );
		}
	}
}