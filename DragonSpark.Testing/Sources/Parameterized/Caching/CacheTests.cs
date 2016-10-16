using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class CacheTests
	{
		[Fact]
		public void Contains()
		{
			var sut = new Cache<object, object>();
			var key = new object();
			Assert.False( sut.Contains( key ) );
			sut.Get( key );
			Assert.True( sut.Contains( key ) );
		}

		[Fact]
		public void GetOrSet()
		{
			var sut = new Cache<Item>( o => new Item( 1 ) );
			var key = new object();
			sut.Get( key );
			var source = sut.GetOrSet( key, o => new Item( 2 ) );
			Assert.Equal( 1, source.Id );
		}

		[Fact]
		public void GetOrSetCreated()
		{
			var sut = new Cache<Item>( o => new Item( 1 ) );
			var key = new object();
			var source = sut.GetOrSet( key, o => new Item( 2 ) );
			Assert.Equal( 2, source.Id );
		}

		sealed class Item
		{
			public Item( int id )
			{
				Id = id;
			}

			public int Id { get; }
		}
	}
}