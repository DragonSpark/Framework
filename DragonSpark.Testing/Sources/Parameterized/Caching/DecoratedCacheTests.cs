using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class DecoratedCacheTests
	{
		[Fact]
		public void Contains()
		{
			var sut = new DecoratedCache<Inner, Wrapper>();
			var key = new Inner();
			Assert.False( sut.Contains( key ) );
			sut.Set( key, new Wrapper( key ) );
			Assert.True( sut.Contains( key ) );
			sut.Remove( key );
			Assert.False( sut.Contains( key ) );
		}

		[Fact]
		public void Create()
		{
			var count = 0;
			var sut = new DecoratedCache<object>( new Cache<object>( o =>
																	 {
																		 ++count;
																		 return new object();
																	 } ) );
			var key = new object();
			Assert.Same( sut.Get( key ), sut.Get( key ) );
			Assert.Equal(1, count);
		}

		[Fact]
		public void Wrapped()
		{
			var sut = new DecoratedCache<Inner, Wrapper>();
			var inner = new Inner();
			var wrapped = sut.Get( inner );
			Assert.Same( inner, wrapped.Inner );
		}

		[Fact]
		public void Instance()
		{
			var sut = new DecoratedCache<Wrapper>( o => new Wrapper( (Inner)o ) );
			var inner = new Inner();
			var wrapped = sut.Get( inner );
			Assert.Same( inner, wrapped.Inner );
		}

		sealed class Inner {}

		sealed class Wrapper
		{
			public Wrapper( Inner inner )
			{
				Inner = inner;
			}

			public Inner Inner { get; }
		}
	}
}