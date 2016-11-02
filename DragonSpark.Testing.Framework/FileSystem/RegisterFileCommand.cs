using DragonSpark.Commands;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public sealed class RegisterFileCommand : CoercedCommand<string, FileSystemRegistration>
	{
		public static RegisterFileCommand Default { get; } = new RegisterFileCommand();
		RegisterFileCommand() : base( FileRegistrationCoercer.Default, Defaults.Register ) {}
	}
}