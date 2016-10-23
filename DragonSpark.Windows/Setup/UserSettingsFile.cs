using DragonSpark.Aspects;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
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

		[UsedImplicitly]
		public UserSettingsFile( Func<ConfigurationUserLevel, string> filePathSource, Func<string, IFileInfo> fileSource )
		{
			this.filePathSource = filePathSource;
			this.fileSource = fileSource;
		}

		[Freeze]
		public override IFileInfo Get( ConfigurationUserLevel parameter ) => fileSource( filePathSource( parameter ) ).Refreshed();
	}
}