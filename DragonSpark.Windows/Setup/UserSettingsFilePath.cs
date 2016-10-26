using DragonSpark.Configuration;
using DragonSpark.Sources;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFilePath : ConfigurableSource<string>
	{
		public static UserSettingsFilePath Default { get; } = new UserSettingsFilePath();
		UserSettingsFilePath() : base( Implementation.DefaultImplementation.Get ) {}

		sealed class Implementation : SourceBase<string>
		{
			public static Implementation DefaultImplementation { get; } = new Implementation();
			Implementation() {}

			public override string Get() => ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.PerUserRoamingAndLocal ).FilePath;
		}
	}
}