using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Windows.FileSystem;
using DragonSpark.Windows.Legacy.Properties;
using JetBrains.Annotations;
using System.Linq;
using Directory = DragonSpark.Windows.FileSystem.Directory;
using File = DragonSpark.Windows.FileSystem.File;
using Path = DragonSpark.Windows.FileSystem.Path;

namespace DragonSpark.Windows.Legacy.Entity
{
	public class InstallDatabaseCommand : RunCommandBase
	{
		readonly IPath path;
		readonly IDirectory directory;
		readonly IFile file;

		readonly static byte[][] Data = { Resources.Blank, Resources.Blank_log };

		public InstallDatabaseCommand() : this( Path.Default, Directory.Default, File.Default ) {}

		public InstallDatabaseCommand( IPath path, IDirectory directory, IFile file )
		{
			this.path = path;
			this.directory = directory;
			this.file = file;
		}

		[Service, PostSharp.Patterns.Contracts.NotNull, UsedImplicitly]
		public IFileInfo Database { [return: PostSharp.Patterns.Contracts.NotNull]get; set; }

		public override void Execute()
		{
			if ( !Database.Exists )
			{
				foreach ( var item in EntityFiles.WithLog( Database ).Tuple( Data ).ToArray() )
				{
					var fullName = item.Item1.FullName;
					var directoryRoot = path.GetDirectoryName( fullName );
					if ( directoryRoot != null )
					{
						directory.CreateDirectory( directoryRoot );
						using ( var stream = file.Create( fullName ) )
						{
							stream.Write( item.Item2, 0, item.Item2.Length );
						}
					}
				}
			}
		}
	}
}