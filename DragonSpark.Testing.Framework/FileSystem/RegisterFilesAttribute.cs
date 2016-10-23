using System;
using System.Linq;
using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application.Setup;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class RegisterFilesAttribute : CommandAttributeBase
	{
		readonly static Action<FileSystemRegistration> Delegate = RegisterFileSystemEntryCommand.Current.Execute;

		public RegisterFilesAttribute( params string[] files ) : base( new CompositeCommand( files.Select( file => Delegate.Fixed( FileSystemRegistration.File( file ) ) ).Fixed() ).Apply( new OncePerScopeSpecification<object>() ) ) {}
	}
}