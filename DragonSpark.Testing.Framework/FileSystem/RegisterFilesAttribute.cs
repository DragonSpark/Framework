using DragonSpark.Commands;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application.Setup;
using System.Collections.Immutable;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class RegisterFilesAttribute : CommandAttributeBase
	{
		readonly static ICommand<ImmutableArray<string>> Command = RegisterFileCommand.Default.AsCompiled<string>().Apply( new OncePerScopeSpecification<ImmutableArray<string>>() );
		
		public RegisterFilesAttribute( params string[] files ) : base( Command.WithParameter( files.ToImmutableArray() ) ) {}
	}
}