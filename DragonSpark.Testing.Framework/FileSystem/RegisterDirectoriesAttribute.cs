using System.Collections.Immutable;
using DragonSpark.Commands;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application.Setup;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class RegisterDirectoriesAttribute : CommandAttributeBase
	{
		readonly static ICommand<ImmutableArray<string>> Command = RegisterDirectoryCommand.Default.AsCompiled<string>().Apply( new OncePerScopeSpecification<ImmutableArray<string>>() );
		
		public RegisterDirectoriesAttribute( params string[] directories ) : base( Command.WithParameter( directories.ToImmutableArray() ) ) {}
	}
}