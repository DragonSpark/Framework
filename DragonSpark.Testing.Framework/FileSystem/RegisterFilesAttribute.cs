using DragonSpark.Coercion;
using DragonSpark.Commands;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application.Setup;
using System.Collections.Immutable;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class RegisterFilesAttribute : CommandAttributeBase
	{
		// readonly static ICommand<ImmutableArray<string>> Command = RegisterFilesCommand.Default;

		public RegisterFilesAttribute( params string[] files ) : base( RegisterFileCommand.Default.AsCompiled<string>().Apply( new OncePerScopeSpecification<ImmutableArray<string>>() ).Fixed( files.ToImmutableArray() ) ) {}
	}

	/*public sealed class RegisterFilesCommand : CompiledCommand<string>
	{
		public static RegisterFilesCommand Default { get; } = new RegisterFilesCommand();
		RegisterFilesCommand() : base( RegisterFileCommand.Default.Execute ) {}
	}*/

	public sealed class RegisterFileCommand : CoercedCommand<string, FileSystemRegistration>
	{
		public static RegisterFileCommand Default { get; } = new RegisterFileCommand();
		RegisterFileCommand() : base( FileSystemRegistrationCoercer.Default, RegisterFileSystemEntryCommand.Current.Execute ) {}
	}

	public sealed class FileSystemRegistrationCoercer : DelegatedCoercer<string, FileSystemRegistration>
	{
		public static FileSystemRegistrationCoercer Default { get; } = new FileSystemRegistrationCoercer();
		FileSystemRegistrationCoercer() : base( FileSystemRegistration.File ) {}
	}
}