using DragonSpark.Commands;
using DragonSpark.Sources;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class RegisterFileSystemEntryCommand : CommandBase<FileSystemRegistration>
	{
		public static IScope<RegisterFileSystemEntryCommand> Current { get; } = Scopes.Create( () => new RegisterFileSystemEntryCommand() );
		RegisterFileSystemEntryCommand() : this( FileSystemRepository.Current.Get() ) {}

		readonly IFileSystemRepository repository;

		[UsedImplicitly]
		public RegisterFileSystemEntryCommand( IFileSystemRepository repository )
		{
			this.repository = repository;
		}

		public override void Execute( FileSystemRegistration parameter ) => repository.Set( parameter.Path, parameter.Element );
	}
}