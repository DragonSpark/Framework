using DragonSpark.Commands;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class RegisterDirectoryCommand : CoercedCommand<string, FileSystemRegistration>
	{
		
		public static RegisterDirectoryCommand Default { get; } = new RegisterDirectoryCommand();
		RegisterDirectoryCommand() : base( DirectoryRegistrationCoercer.Default, Defaults.Register ) {}
	}
}