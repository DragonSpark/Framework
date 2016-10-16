using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class DecoratedSourceCacheTests
	{
		[Fact]
		public void Contains()
		{
			var sut = new DecoratedSourceCache<object, object>( o => new object() );
			var key = new object();
			Assert.False( sut.Contains( key ) );
			sut.Get( key );
			Assert.True( sut.Contains( key ) );
		}

		[Fact]
		public void ContainsGeneralized()
		{
			var sut = new DecoratedSourceCache<object>( o => new object() );
			var key = new object();
			Assert.False( sut.Contains( key ) );
			sut.Get( key );
			Assert.True( sut.Contains( key ) );
		}
	}
}