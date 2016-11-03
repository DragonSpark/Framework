using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class CachesTests
	{
		[Fact]
		public void Create()
		{
			var sut = Caches.Create( () => new object() );
			var key = new object();
			var created = sut.Get( key );
			Assert.NotSame( created, key );
			Assert.Same( created, sut.Get( key ) );
		}
	}
}