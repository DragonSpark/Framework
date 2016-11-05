using DragonSpark.Windows.Setup;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup
{
	public class UserSettingsFilePathTests
	{
		[Fact]
		public void Coverage()
		{
			Assert.NotNull( UserSettingsFilePath.DefaultImplementation.Implementation.Get() );
		}
	}
}