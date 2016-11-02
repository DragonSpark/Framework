using DragonSpark.Commands;
using JetBrains.Annotations;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class RegisterFileSystemEntryCommand : CommandBase<FileSystemRegistration>
	{
		public static RegisterFileSystemEntryCommand Default { get; } = new RegisterFileSystemEntryCommand();
		RegisterFileSystemEntryCommand() : this( FileSystemRepository.Default ) {}

		readonly IFileSystemRepository repository;

		[UsedImplicitly]
		public RegisterFileSystemEntryCommand( IFileSystemRepository repository )
		{
			this.repository = repository;
		}

		public override void Execute( FileSystemRegistration parameter ) => repository.Set( parameter.Path, parameter.Element );
	}
}