using DragonSpark.Sources;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	sealed class UserSettingsFilePath : ScopedSource<string>
	{
		public static UserSettingsFilePath Default { get; } = new UserSettingsFilePath();
		UserSettingsFilePath() : base( DefaultImplementation.Implementation.Get ) {}

		public sealed class DefaultImplementation : SourceBase<string>
		{
			[UsedImplicitly]
			public const string UserSettingsPath = @"UserSettings\CurrentVersion", UserConfigurationFileName = "user.config";

			public static DefaultImplementation Implementation { get; } = new DefaultImplementation();
			DefaultImplementation() : this( Path.Default, DirectorySource.Default ) {}

			readonly IPath path;
			readonly IDirectorySource directory;
			readonly string directoryName;
			readonly string fileName;

			[UsedImplicitly]
			public DefaultImplementation( IPath path, IDirectorySource directory, string directoryName = UserSettingsPath, string fileName = UserConfigurationFileName )
			{
				this.path = path;
				this.directory = directory;
				this.directoryName = directoryName;
				this.fileName = fileName;
			}

			public override string Get() => path.Combine( directory.Get(), directoryName, fileName );
		}
	}
}