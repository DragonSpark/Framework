using DragonSpark.Aspects;
using DragonSpark.Configuration;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;
using System;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public sealed class UserSettingsFile : ParameterizedSourceBase<ConfigurationUserLevel, IFileInfo>
	{
		public static UserSettingsFile Default { get; } = new UserSettingsFile();
		UserSettingsFile() : this( UserSettingsFilePath.Default.Get, FileInfoFactory.Default.Get ) {}

		readonly Func<ConfigurationUserLevel, string> filePathSource;
		readonly Func<string, IFileInfo> fileSource;

		UserSettingsFile( Func<ConfigurationUserLevel, string> filePathSource, Func<string, IFileInfo> fileSource )
		{
			this.filePathSource = filePathSource;
			this.fileSource = fileSource;
		}

		[Freeze]
		public override IFileInfo Get( ConfigurationUserLevel parameter ) => fileSource( filePathSource( parameter ) ).Refreshed();
	}

	public sealed class UserSettingsFilePath : ConfigurableParameterizedSource<ConfigurationUserLevel, string>
	{
		public static UserSettingsFilePath Default { get; } = new UserSettingsFilePath();
		UserSettingsFilePath() : base( level => ConfigurationManager.OpenExeConfiguration( level ).FilePath ) {}
	}
}