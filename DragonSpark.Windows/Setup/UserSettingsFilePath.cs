using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFilePath : ConfigurableSource<string>
	{
		public static UserSettingsFilePath Default { get; } = new UserSettingsFilePath();
		UserSettingsFilePath() : base( DefaultImplementation.Implementation.Get ) {}

		sealed class DefaultImplementation : SourceBase<string>
		{
			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() {}

			public override string Get() => ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.PerUserRoamingAndLocal ).FilePath;
		}
	}
}