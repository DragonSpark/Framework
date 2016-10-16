using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class CacheExtensionsTests
	{
		[Fact]
		public void SetOrClear()
		{
			var cache = new Cache<object>();
			var key = new object();
			var value = new object();
			cache.SetOrClear( key, value );
			Assert.Same( value, cache.Get( key ) );

			cache.SetOrClear( key );
			Assert.Null( cache.Get( key ) );
		}
	}
}