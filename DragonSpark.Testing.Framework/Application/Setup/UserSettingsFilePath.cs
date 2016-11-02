using DragonSpark.Sources;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	sealed class UserSettingsFilePath : SourceBase<string>
	{
		[UsedImplicitly]
		public const string UserSettingsPath = @"UserSettings\CurrentVersion", UserConfigurationFileName = "user.config";

		public static UserSettingsFilePath Default { get; } = new UserSettingsFilePath();
		UserSettingsFilePath() : this( Path.Default, DirectorySource.Default ) {}

		readonly IPath path;
		readonly IDirectorySource directory;
		readonly string directoryName;
		readonly string fileName;

		[UsedImplicitly]
		public UserSettingsFilePath( IPath path, IDirectorySource directory, string directoryName = UserSettingsPath, string fileName = UserConfigurationFileName )
		{
			this.path = path;
			this.directory = directory;
			this.directoryName = directoryName;
			this.fileName = fileName;
		}

		public override string Get() => path.Combine( directory.Get(), directoryName, fileName );
	}
}