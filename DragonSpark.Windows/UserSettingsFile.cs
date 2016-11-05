using System;
using DragonSpark.Sources;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;

namespace DragonSpark.Windows
{
	public sealed class UserSettingsFile : SourceBase<IFileInfo>
	{
		public static UserSettingsFile Default { get; } = new UserSettingsFile();
		UserSettingsFile() : this( UserSettingsFilePath.Default.Get, FileInfoFactory.Default.Get ) {}

		readonly Func<string> filePathSource;
		readonly Func<string, IFileInfo> fileSource;

		[UsedImplicitly]
		public UserSettingsFile( Func<string> filePathSource, Func<string, IFileInfo> fileSource )
		{
			this.filePathSource = filePathSource;
			this.fileSource = fileSource;
		}

		public override IFileInfo Get() => fileSource( filePathSource() ).Refreshed();
	}
}