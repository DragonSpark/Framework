using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;
using System;

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

		public UserSettingsFilePath( IPath path, IDirectorySource directory, string directoryName = UserSettingsPath, string fileName = UserConfigurationFileName )
		{
			this.path = path;
			this.directory = directory;
			this.directoryName = directoryName;
			this.fileName = fileName;
		}

		public override string Get() => path.Combine( directory.Get(), directoryName, fileName );
	}

	[ApplyAutoValidation, ApplySpecification( typeof(OnlyOnceSpecification) )]
	public sealed class InitializeUserSettingsFileCommand : SuppliedCommand<FileSystemEntry>
	{
		readonly static Action<FileSystemEntry> Delegate = ApplyFileSystemEntryCommand.Current.Delegate();

		public static IScope<InitializeUserSettingsFileCommand> Current { get; } = new Scope<InitializeUserSettingsFileCommand>( Factory.GlobalCache( () => new InitializeUserSettingsFileCommand() ) );
		InitializeUserSettingsFileCommand() : this( UserSettingsFilePath.Current.Get().Get() ) {}

		public InitializeUserSettingsFileCommand( string userSettingsFilePath ) : base( Delegate, FileSystemEntry.File( userSettingsFilePath ) ) {}

		public sealed class Attribute : ExecutedRunCommandAttributeBase
		{
			public Attribute() : base( Current.Get() ) {}
		}
	}
}
