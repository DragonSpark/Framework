using DragonSpark.Sources.Scopes;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFilePath : ScopedSingleton<string>
	{
		public static UserSettingsFilePath Default { get; } = new UserSettingsFilePath();
		UserSettingsFilePath() : base( () => ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.PerUserRoamingAndLocal ).FilePath ) {}
	}
}