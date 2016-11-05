using System.Globalization;
using Xunit;

namespace DragonSpark.Testing.Properties
{
	public class ResourcesTests
	{
		[Fact]
		void Coverage()
		{
			DragonSpark.Properties.Resources.Culture = CultureInfo.CurrentCulture;
			Assert.Equal( CultureInfo.CurrentCulture, DragonSpark.Properties.Resources.Culture );
			Assert.NotNull( DragonSpark.Properties.Resources.INVALID_CHARACTERS );
			Assert.NotNull( DragonSpark.Properties.Resources.SERVER_PATH );
		} 
	}
}