using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.Application.Setup;
using Xunit;

namespace DragonSpark.Windows.Testing
{
	public class UserConfigurationLocatorTests
	{
		[Theory, AutoData, InitializeUserSettingsFile]
		void Verify( UserSettingsFile path, UserConfigurationLocator sut )
		{
			Assert.Null( sut.Get( path.Get() ) );
		}
	}
}