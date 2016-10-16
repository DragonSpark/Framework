using DragonSpark.Sources.Parameterized.Caching;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized.Caching
{
	public class StackTests
	{
		[Fact]
		public void Contains()
		{
			var sut = new Stack<object>();
			var item = new object();
			Assert.False( sut.Contains( item ) );
			sut.Push( item );

			Assert.True( sut.Contains( item ) );
			Assert.Same( item, sut.Pop() );
			Assert.False( sut.Contains( item ) );
		}
	}
}