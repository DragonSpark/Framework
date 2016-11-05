using Xunit;

namespace DragonSpark.Windows.Testing
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