using DragonSpark.Coercion;
using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application.Setup;
using JetBrains.Annotations;
using System.Collections.Immutable;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class RegisterFilesAttribute : CommandAttributeBase
	{
		readonly static ICommand<ImmutableArray<string>> Command = RegisterFileCommand.Default.AsCompiled<string>().Apply( new OncePerScopeSpecification<ImmutableArray<string>>() );
		
		public RegisterFilesAttribute( params string[] files ) : base( Command.ToCommand( files.ToImmutableArray() ) ) {}
	}

	public class RegisterDirectoriesAttribute : CommandAttributeBase
	{
		readonly static ICommand<ImmutableArray<string>> Command = RegisterDirectoryCommand.Default.AsCompiled<string>().Apply( new OncePerScopeSpecification<ImmutableArray<string>>() );
		
		public RegisterDirectoriesAttribute( params string[] directories ) : base( Command.ToCommand( directories.ToImmutableArray() ) ) {}
	}

	public sealed class InitializeFileSystemAttribute : RegisterDirectoriesAttribute
	{
		public InitializeFileSystemAttribute() : this( DirectorySource.Current.GetValue() ) {}

		[UsedImplicitly]
		public InitializeFileSystemAttribute( string rootDirectory ) : base( rootDirectory ) {}
	}

	/*public sealed class InitializeFileSystemCommand : SuppliedCommand<string>
	{
		public static IScope<InitializeFileSystemCommand> Current { get; } = new Scope<InitializeFileSystemCommand>( Factory.GlobalCache( () => new InitializeFileSystemCommand() ) );
		InitializeFileSystemCommand() : this( DirectorySource.Current.Get() ) {}

		public InitializeFileSystemCommand( IDirectorySource source ) : base( RegisterDirectoryCommand.Default, source.Get ) {}
	}*/

	public sealed class RegisterFileCommand : CoercedCommand<string, FileSystemRegistration>
	{
		public static RegisterFileCommand Default { get; } = new RegisterFileCommand();
		RegisterFileCommand() : base( FileRegistrationCoercer.Default, RegisterFileSystemEntryCommand.Current.Execute ) {}
	}

	public sealed class RegisterDirectoryCommand : CoercedCommand<string, FileSystemRegistration>
	{
		public static RegisterDirectoryCommand Default { get; } = new RegisterDirectoryCommand();
		RegisterDirectoryCommand() : base( DirectoryRegistrationCoercer.Default, RegisterFileSystemEntryCommand.Current.Execute ) {}
	}

	public sealed class FileRegistrationCoercer : DelegatedCoercer<string, FileSystemRegistration>
	{
		public static FileRegistrationCoercer Default { get; } = new FileRegistrationCoercer();
		FileRegistrationCoercer() : base( FileSystemRegistration.File ) {}
	}

	public sealed class DirectoryRegistrationCoercer : DelegatedCoercer<string, FileSystemRegistration>
	{
		public static DirectoryRegistrationCoercer Default { get; } = new DirectoryRegistrationCoercer();
		DirectoryRegistrationCoercer() : base( FileSystemRegistration.Directory ) {}
	}
}