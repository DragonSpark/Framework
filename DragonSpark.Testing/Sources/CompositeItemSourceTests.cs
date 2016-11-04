using DragonSpark.Sources;
using Xunit;

namespace DragonSpark.Testing.Sources
{
	public class CompositeItemSourceTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.Empty( new CompositeItemSource<object>() );
		}
	}
}