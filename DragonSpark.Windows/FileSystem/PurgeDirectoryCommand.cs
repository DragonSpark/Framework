using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using System;
using System.Collections.Immutable;
using System.IO;

namespace DragonSpark.Windows.FileSystem
{
	[ApplyAutoValidation, ApplySpecification( typeof(FileSystemInfoExistsSpecification) )]
	public sealed class PurgeDirectoryCommand : CommandBase<IDirectoryInfo>
	{
		public static PurgeDirectoryCommand Default { get; } = new PurgeDirectoryCommand();
		PurgeDirectoryCommand() : this( AllFilesSource.Default.Get ) {}

		readonly Func<IDirectoryInfo, ImmutableArray<IFileInfo>> fileSource;
		readonly Action<IDirectoryInfo> delete;

		public PurgeDirectoryCommand( Func<IDirectoryInfo, ImmutableArray<IFileInfo>> fileSource )
		{
			this.fileSource = fileSource;
			delete = TryDeleteDirectory;
		}

		public override void Execute( IDirectoryInfo parameter )
		{
			parameter.GetDirectories().Each( delete );
			parameter.GetFiles().Each( x => x.Delete() );
		}

		void TryDeleteDirectory( IDirectoryInfo target )
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