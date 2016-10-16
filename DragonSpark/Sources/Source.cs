using DragonSpark.Sources.Parameterized.Caching;

namespace DragonSpark.Sources
{
	public static class Source
	{
		public static Source<T> Sourced<T>( this T @this ) => Support<T>.Sources.Get( @this );

		static class Support<T>
		{
			public static ICache<T, Source<T>> Sources { get; } = CacheFactory.Create<T, Source<T>>( arg => new Source<T>( arg ) );
		}
	}
}