using DragonSpark.Windows.FileSystem;
using System.Collections.Generic;
using System.IO;

namespace DragonSpark.Windows.Legacy.Entity
{
	public static class EntityFiles
	{
		public static IDirectoryInfo DefaultDataDirectory { get; } = DirectoryFactory.Default.Get( @".\App_Data" );

		public static IEnumerable<IFileInfo> WithLog( IFileInfo databaseFile ) => new[] { databaseFile, GetLog( databaseFile ) };

		public static IFileInfo GetLog( IFileInfo database ) => FileFactory.Default.Get( Path.Combine( database.DirectoryName ?? string.Empty, string.Concat( Path.GetFileNameWithoutExtension( database.Name ), "_log.ldf" ) ) );
	}
}