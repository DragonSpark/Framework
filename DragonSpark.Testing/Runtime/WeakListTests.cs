using DragonSpark.Runtime;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class WeakListTests
	{
		[Fact]
		public void Get()
		{
			var item = new object();
			var sut = new WeakList<object>
					  {
						  item
					  };

			Assert.True( sut.Contains( item ) );
			Assert.False( sut.Contains( new object() ) );
		}
	}
}