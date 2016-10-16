using DragonSpark.Sources.Parameterized;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized
{
	public class SelfAlterationTests
	{
		[Fact]
		public void Coverage()
		{
			var item = new object();
			Assert.Same( item, SelfAlteration<object>.Default.Get( item ) );
		}
	}
}