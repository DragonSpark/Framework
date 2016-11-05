using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFilePath : SingletonScope<string>
	{
		public static UserSettingsFilePath Default { get; } = new UserSettingsFilePath();
		UserSettingsFilePath() : base( DefaultImplementation.Implementation.Get ) {}

		public sealed class DefaultImplementation : SourceBase<string>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() {}

			public override string Get() => 
				ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.PerUserRoamingAndLocal ).FilePath;
		}
	}
}