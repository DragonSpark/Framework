using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Windows.Setup;
using System;
using System.Collections.Immutable;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	[ApplyAutoValidation, ApplySpecification( typeof(FileSystemInfoExistsSpecification) )]
	public sealed class PurgeDirectoryCommand : CommandBase<DirectoryInfoBase>
	{
		public static PurgeDirectoryCommand Default { get; } = new PurgeDirectoryCommand();
		PurgeDirectoryCommand() : this( AllFilesSource.Default.Get ) {}

		readonly Func<DirectoryInfoBase, ImmutableArray<FileInfoBase>> fileSource;
		readonly Action<DirectoryInfoBase> delete;

		public PurgeDirectoryCommand( Func<DirectoryInfoBase, ImmutableArray<FileInfoBase>> fileSource )
		{
			this.fileSource = fileSource;
			delete = TryDeleteDirectory;
		}

		public override void Execute( DirectoryInfoBase parameter )
		{
			parameter.GetDirectories().Each( delete );
			parameter.GetFiles().Each( x => x.Delete() );
		}

		void TryDeleteDirectory( DirectoryInfoBase target )
		{
			try
			{
				target.Delete( true );
			}
			catch ( IOException )
			{
				foreach ( var file in fileSource( target ) )
				{
					try
					{
						file.Delete();
					}
					catch ( Exception exception )
					{
						Logger.Default.Get( file ).Error( exception, "Could not delete {File}.", file.FullName );
					}
				}
			}
		}
	}
}