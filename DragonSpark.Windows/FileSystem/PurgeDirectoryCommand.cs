using System;
using System.Collections.Immutable;
using System.IO;
using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Windows.Setup;

namespace DragonSpark.Windows.FileSystem
{
	[ApplyAutoValidation, ApplySpecification( typeof(FileSystemInfoExistsSpecification) )]
	public sealed class PurgeDirectoryCommand : CommandBase<DirectoryInfo>
	{
		public static PurgeDirectoryCommand Default { get; } = new PurgeDirectoryCommand();
		PurgeDirectoryCommand() : this( AllFilesSource.Default.Get ) {}

		readonly Func<DirectoryInfo, ImmutableArray<FileInfo>> fileSource;
		readonly Action<DirectoryInfo> delete;

		public PurgeDirectoryCommand( Func<DirectoryInfo, ImmutableArray<FileInfo>> fileSource  )
		{
			this.fileSource = fileSource;
			delete = TryDeleteDirectory;
		}

		public override void Execute( DirectoryInfo parameter )
		{
			parameter.GetDirectories().Each( delete );
			parameter.GetFiles().Each( x => x.Delete() );
		}

		void TryDeleteDirectory( DirectoryInfo target )
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