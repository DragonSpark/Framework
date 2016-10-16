using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class ThreadLocalSourceCacheTests
	{
		[Fact]
		public void Coverage_Create()
		{
			var sut = new ThreadLocalSourceCache<object>();
			var key = new object();
			var source = sut.Get( key );
			Assert.NotNull( source );
			Assert.Same( sut.Get( key ), sut.Get( key ) );
		}

		[Fact]
		public void Coverage_CreateWithDelegate()
		{
			var sut = new ThreadLocalSourceCache<object, object>( () => new object() );
			var key = new object();
			Assert.Same( sut.Get( key ), sut.Get( key ) );
		}

		[Fact]
		public void Coverage_CreateWithDelegateGeneralized()
		{
			var sut = new ThreadLocalSourceCache<object>( () => new object() );
			var key = new object();
			Assert.Same( sut.Get( key ), sut.Get( key ) );
		}
	}
}