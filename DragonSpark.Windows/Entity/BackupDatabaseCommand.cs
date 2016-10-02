using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System.IO;
using System.Linq;

namespace DragonSpark.Windows.Entity
{
	public class BackupDatabaseCommand : CommandBase<object>
	{
		[Service, NotNull]
		public FileInfo Database { [return: NotNull]get; set; }

		[Default( 6 )]
		public int? MaximumBackups { get; set; }

		public override void Execute( object parameter )
		{
			var files = EntityFiles.WithLog( Database ).Where( info => !info.IsLocked() ).ToArray();
			var directory = Database.Directory;
			if ( files.Any() )
			{
				var destination = directory.CreateSubdirectory( FileSystem.GetValidPath() );
				foreach ( var file in files )
				{
					file.CopyTo( Path.Combine( destination.FullName, file.Name ) );
				}
			}

			if ( MaximumBackups.HasValue )
			{
				directory
						.GetDirectories()
						.Where( x => FileSystem.IsValidPath( x.Name ) )
						.OrderByDescending( info => info.CreationTime )
						.Skip( MaximumBackups.Value )
						.Each( info => info.Delete( true ) );
			}
		}
	}
}