using DragonSpark.Aspects;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Windows.Io;
using System.IO;
using System.Linq;

namespace DragonSpark.Windows.Entity
{
	public class BackupDatabaseCommand : SetupCommandBase
	{
		[Factory( typeof(AttachedDatabaseFileFactory) )]
		public FileInfo Database { get; set; }

		[Default( 6 )]
		public int? MaximumBackups { get; set; }

		[BuildUp]
		protected override void OnExecute( object parameter )
		{
			Database.With( file =>
			{
				var files = EntityFiles.WithLog( Database ).Where( info => !info.IsLocked() ).ToArray();
				files.Any().IsTrue( () =>
				{
					var destination = file.Directory.CreateSubdirectory( FileSystem.GetValidPath() );
					files.Each( info => info.CopyTo( Path.Combine( destination.FullName, info.Name ) ) );
				} );

				MaximumBackups.With( i => 
					file.Directory
						.GetDirectories()
						.Where( x => FileSystem.IsValidPath( x.Name ) )
						.OrderByDescending( info => info.CreationTime )
						.Skip( i )
						.Each( info => info.Delete( true ) ) 
					);
			} );
		}
	}
}