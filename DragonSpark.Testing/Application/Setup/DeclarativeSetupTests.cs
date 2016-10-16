using DragonSpark.Application.Setup;
using Xunit;

namespace DragonSpark.Testing.Application.Setup
{
	public class DeclarativeSetupTests
	{
		[Fact]
		public void Coverage()
		{
			var sut = new DeclarativeSetup();
			Assert.Empty( sut.Items );
		}
	}
}