using System.Configuration;
using DragonSpark.Configuration;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFilePath : ConfigurableParameterizedSource<ConfigurationUserLevel, string>
	{
		public static UserSettingsFilePath Default { get; } = new UserSettingsFilePath();
		UserSettingsFilePath() : base( level => ConfigurationManager.OpenExeConfiguration( level ).FilePath ) {}
	}
}