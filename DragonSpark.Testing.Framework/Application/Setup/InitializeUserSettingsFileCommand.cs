using DragonSpark.Sources;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	sealed class UserSettingsFilePath : SourceBase<string>
	{
		const string UserSettingsPath = @"UserSettings\CurrentVersion", UserConfigurationFileName = "user.config";

		public static IScope<UserSettingsFilePath> Current { get; } = new Scope<UserSettingsFilePath>( Factory.GlobalCache( () => new UserSettingsFilePath() ) );
		UserSettingsFilePath() : this( Path.Current.Get(), DirectorySource.Current.Get() ) {}

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