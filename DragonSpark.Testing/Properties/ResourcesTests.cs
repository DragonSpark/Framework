using System.Globalization;
using Xunit;

namespace DragonSpark.Testing.Properties
{
	public class ResourcesTests
	{
		[Fact]
		void Resources()
		{
			DragonSpark.Properties.Resources.Culture = CultureInfo.CurrentCulture;
			Assert.Equal( CultureInfo.CurrentCulture, DragonSpark.Properties.Resources.Culture );
		} 
	}
}